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
    [SerializeField] private float standardShotThreshold = 0.5f;

    [HideInInspector] public float chargevalue = 0;

    private bool charging = false;
    private bool canFire = true;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Fire.performed += context => Fire();
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
            canFire = true;

        if (value == 0)
            canFire = false;
    }

    private void MuzzleFlash(GameObject muzzleFlash)
    {
        Instantiate(muzzleFlash, projectileSpawner.position, projectileSpawner.rotation, projectileSpawner.transform);
    }

    private void Fire()
    {
        Actions.OnFireNormal();

        if (canFire)
        {
            Instantiate(powerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
            MuzzleFlash(powerBeamMuzzleFlash);
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

        float totalTime = 0;
        while (totalTime <= chargeTime && charging == true)
        {
            chargevalue = totalTime / chargeTime;
            totalTime += Time.deltaTime;
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
            Fire();
        else
            FireCharged();

        charging = false;
        chargevalue = 0;
          
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
