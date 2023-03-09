using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerInput playerInput;
    [HideInInspector] public float chargevalue = 0;
    [HideInInspector] public bool canFireNormalShot = true;
    public enum ActiveBeam { Power, Wave, Ice, Plasma};
    public ActiveBeam activeBeam;

    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private GameObject powerBeamProjectile;
    [SerializeField] private GameObject waveBeamProjectile;
    [SerializeField] private GameObject iceBeamProjectile;
    [SerializeField] private GameObject plasmaBeamProjectile;
    [SerializeField] private GameObject powerBeamMuzzleFlash;
    [SerializeField] private GameObject missileProjectile;
    [SerializeField] private float missileFireCooldownTime = 1.0f;
    [SerializeField] private GameObject chargedPowerBeamProjectile;
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
            canFireNormalShot = true;

        if (value == 0)
            canFireNormalShot = false;
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
    }

    private void FireNormal()
    {
        if (canFireNormalShot)
        {
            Actions.OnFireNormal();

            switch (activeBeam)
            {
                //TODO make this a function
                case ActiveBeam.Power:
                    Instantiate(powerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    break;
                case ActiveBeam.Wave:
                    Instantiate(waveBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    break;
                case ActiveBeam.Ice:
                    Instantiate(iceBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    break;
                case ActiveBeam.Plasma:
                    Instantiate(plasmaBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
                    MuzzleFlash(powerBeamMuzzleFlash);
                    break;
            }
        }
    }

    private void FireMissle()
    {
        if (canFireNormalShot)
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
            chargevalue = totalChargeTime / chargeTime;
            totalChargeTime += Time.deltaTime;

            if(chargevalue >= standardShotThreshold)
                canFireNormalShot = false;

            if (chargevalue > 0.99f)
                chargevalue = 1.0f;

            yield return null;
        }
    }

    private IEnumerator ChargeCooldown()
    {
        totalChargeTime = totalChargeTime / chargeTime * chargeCooldownTime;

        while (totalChargeTime > 0 && charging == false)
        {
            chargevalue = totalChargeTime / chargeCooldownTime;
            totalChargeTime -= Time.deltaTime;

            if (chargevalue < 0.015f)
            {
                chargevalue = 0;
                canFireNormalShot = true;
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

        if(chargevalue <= standardShotThreshold)
            FireNormal();
        else
            FireCharged();

        charging = false;
        StartCoroutine(ChargeCooldown());
    }

    private void FireCharged()
    {
        Actions.OnFireCharged();
        GameObject chargedShot = Instantiate(chargedPowerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
        chargedShot.transform.localScale = new Vector3(chargevalue, chargevalue, chargevalue);
        //TODO Change this to a larger muzzle flash
        MuzzleFlash(powerBeamMuzzleFlash);
    }
}
