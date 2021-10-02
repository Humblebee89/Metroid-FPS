using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private GameObject powerBeamProjectile;
    [SerializeField] private GameObject chargedPowerBeamProjectile;
    [SerializeField] private float chargeTime = 1.0f;
    [SerializeField] private float standardShotThreshold = 0.5f;

    [HideInInspector] public float chargevalue = 0;

    private bool charging = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Fire.performed += context => Fire();
        playerInput.Player.ChargeStart.performed += context => StartCoroutine("ChargeStart");
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

    private void Fire()
    {
        Actions.OnFireNormal();
        Instantiate(powerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
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

    private void ChargeEnd()
    {
        if(charging == false) // Prevents this from firing with a standard shot
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
        GameObject chargedShot = Instantiate(chargedPowerBeamProjectile, projectileSpawner.position, projectileSpawner.rotation);
        chargedShot.transform.localScale = new Vector3(chargevalue, chargevalue, chargevalue);
    }
}
