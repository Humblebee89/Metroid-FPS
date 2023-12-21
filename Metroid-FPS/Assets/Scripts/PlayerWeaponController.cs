using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerInput playerInput;
    [HideInInspector] public float chargeValue = 0;
    [HideInInspector] public bool canFire = true;
    public enum ActiveBeam { Power, Wave, Ice, Plasma};
    public ActiveBeam activeBeam;

    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private GameObject powerBeamMuzzleFlash;
    [SerializeField] private GameObject powerBeamProjectile;
    [SerializeField] private GameObject chargedPowerBeamProjectile;
    [SerializeField] private float powerBeamShotDelay;
    [SerializeField] private GameObject waveBeamMuzzleFlash;
    [SerializeField] private GameObject waveBeamProjectile;
    [SerializeField] private GameObject chargedWaveBeamProjectile;
    [SerializeField] private float waveBeamShotDelay;
    [SerializeField] private GameObject iceBeamProjectile;
    [SerializeField] private GameObject chargedIceBeamProjectile;
    [SerializeField] private float iceBeamShotDelay;
    [SerializeField] private GameObject plasmaBeamProjectile;
    [SerializeField] private GameObject chargedPlasmaBeamProjectile;
    [SerializeField] private float plasmaBeamShotDelay;
    [SerializeField] private GameObject missileProjectile;
    [SerializeField] private float missileFireCooldownTime = 1.0f;
    [SerializeField] private float chargeTime = 1.0f;
    [SerializeField] private float chargeCooldownTime = 1.0f;
    [SerializeField] private float standardShotThreshold = 0.5f;

    
    private float totalChargeTime = 0;

    [HideInInspector] public bool charging = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Fire.performed += context => FireNormal();
        playerInput.Player.FireMissile.performed += context => FireMissle();
        playerInput.Player.ChargeStart.performed += context => StartCoroutine("ChargeStart");
        playerInput.Player.ChargeStart.performed += context => ChargeStarted();
        playerInput.Player.ChargeEnd.performed += context => ChargeEnd();
        playerInput.Player.SwapBeamPower.performed += context => SwapBeam(ActiveBeam.Power);
        playerInput.Player.SwapBeamWave.performed += context => SwapBeam(ActiveBeam.Wave);
        playerInput.Player.SwapBeamIce.performed += context => SwapBeam(ActiveBeam.Ice);
        playerInput.Player.SwapBeamPlasma.performed += context => SwapBeam(ActiveBeam.Plasma);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void CanFire(int value)
    {
        if (value == 1)
            canFire = true;

        if (value == 0)
            canFire = false;
    }

    private void MuzzleFlash(GameObject muzzleFlash)
    {
        Instantiate(muzzleFlash, projectileSpawner.position, projectileSpawner.rotation, projectileSpawner.transform);
    }

    private void SwapBeam(ActiveBeam beam)
    {
        if (beam == activeBeam)
            return;

        activeBeam = beam;
        Actions.OnBeamChange();

        BeamSwapChargeEnd();
    }

    IEnumerator ShotDelay(float waitTime)
    {
        canFire = false;
        yield return new WaitForSeconds(waitTime);
        canFire = true;
    }

    private void FireNormal()
    {
        if (canFire)
        {
            Actions.OnFireNormal();

            switch (activeBeam)
            {
                //TODO make this a function
                case ActiveBeam.Power:
                    Instantiate(powerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    StartCoroutine(ShotDelay(powerBeamShotDelay));
                    break;
                case ActiveBeam.Wave:
                    Instantiate(waveBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(waveBeamMuzzleFlash);
                    StartCoroutine(ShotDelay(waveBeamShotDelay));
                    break;
                case ActiveBeam.Ice:
                    Instantiate(iceBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    StartCoroutine(ShotDelay(iceBeamShotDelay));
                    break;
                case ActiveBeam.Plasma:
                    Instantiate(plasmaBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    StartCoroutine(ShotDelay(plasmaBeamShotDelay));
                    break;
            }
        }
    }

    private void FireMissle()
    {
        if (canFire)
        {
            Actions.OnFireMissile();
            Instantiate(missileProjectile, projectileSpawner.position, projectileSpawner.rotation);
            //TODO Make a Missile specific muzzle flash
            MuzzleFlash(powerBeamMuzzleFlash);
        }
    }

    private IEnumerator ChargeStart()
    {
        charging = true;
        totalChargeTime = 0;

        while (totalChargeTime <= chargeTime && charging == true)
        {
            chargeValue = totalChargeTime / chargeTime;
            totalChargeTime += Time.deltaTime;

            if(chargeValue >= standardShotThreshold)
                canFire = false;

            if (chargeValue > 0.99f)
                chargeValue = 1.0f;

            yield return null;
        }
    }

    private IEnumerator ChargeCooldown()
    {
        totalChargeTime = totalChargeTime / chargeTime * chargeCooldownTime;

        while (totalChargeTime > 0 && charging == false)
        {
            chargeValue = totalChargeTime / chargeCooldownTime;
            totalChargeTime -= Time.deltaTime;

            if (chargeValue < 0.02f)
            {
                chargeValue = 0;
                canFire = true;
                Actions.OnChargeCooldownEnd();
            }

            yield return null;
        }
    }

    private void ChargeStarted()
    {
        Actions.OnChargeStarted();
    }

    private void ChargeEnd()
    {
        if(charging == false) // Prevents this from being called with a standard shot
        {
            return;
        }

        if(chargeValue <= standardShotThreshold)
            FireNormal();
        else
            FireCharged();

        charging = false;
        StartCoroutine(ChargeCooldown());
    }

    private void BeamSwapChargeEnd()
    {
        charging = false;
        StartCoroutine(ChargeCooldown());
    }

    private void FireCharged()
    {
        Actions.OnFireCharged();

        GameObject chargedShot = null;

        switch (activeBeam)
        {
            //TODO make this a function
            case ActiveBeam.Power:
                chargedShot = Instantiate(chargedPowerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                chargedShot.transform.localScale = new Vector3(chargeValue, chargeValue, chargeValue);
                //TODO Change this to a larger muzzle flash
                MuzzleFlash(powerBeamMuzzleFlash);
                break;
            case ActiveBeam.Wave:
                chargedShot = Instantiate(chargedWaveBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                chargedShot.transform.localScale = new Vector3(chargeValue, chargeValue, chargeValue);
                MuzzleFlash(powerBeamMuzzleFlash);
                break;
            case ActiveBeam.Ice:
                chargedShot = Instantiate(chargedIceBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                chargedShot.transform.localScale = new Vector3(chargeValue, chargeValue, chargeValue);
                MuzzleFlash(powerBeamMuzzleFlash);
                break;
            case ActiveBeam.Plasma:
                chargedShot = Instantiate(chargedPlasmaBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                chargedShot.transform.localScale = new Vector3(chargeValue, chargeValue, chargeValue);
                MuzzleFlash(powerBeamMuzzleFlash);
                break;
        }
    }
}
