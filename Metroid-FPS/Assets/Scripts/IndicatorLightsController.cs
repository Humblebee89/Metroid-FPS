using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLightsController : MonoBehaviour
{
    [SerializeField] private Material lightMaterial;

    private PlayerWeaponController playerWeaponController;
    private bool charging;

    private void OnEnable()
    {
        Actions.OnChargeStarted += ChargeStarted;
        Actions.OnChargeCooldownEnd += ChargeCooldownEnd;
    }
    private void OnDisable()
    {
        Actions.OnChargeStarted -= ChargeStarted;
        Actions.OnChargeCooldownEnd -= ChargeCooldownEnd;
    }

    private void Awake()
    {
        playerWeaponController = GetComponent<PlayerWeaponController>();
    }

    private void ChargeStarted()
    {
        charging = true;
    }

    private void ChargeCooldownEnd()
    {
        charging = false;
    }

    private void Update()
    {
        if (!charging)
            return;
        else
        {
            lightMaterial.SetFloat("_Fill", playerWeaponController.chargeValue);
        }
    }



}
