using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private GameObject powerBeamProjectile;
    [SerializeField] private GameObject powerBeamMuzzleFlash;
    [SerializeField] private GameObject missileProjectile;
    [SerializeField] private float missileFireCooldownTime = 1.0f;
    [SerializeField] private GameObject chargedPowerBeamProjectile;
    [SerializeField] private float chargeTime = 1.0f;
    [SerializeField] private float chargeCooldownTime = 1.0f;
    [SerializeField] private float standardShotThreshold = 0.5f;

    [HideInInspector] public float chargevalue = 0;
 
    private bool canFireNormalShot = true;
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

    private void FireNormal()
    {
        if (canFireNormalShot)
        {
            Actions.OnFireNormal();
            Instantiate(powerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
            MuzzleFlash(powerBeamMuzzleFlash);
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

            if (chargevalue < 0.01f)
            {
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
