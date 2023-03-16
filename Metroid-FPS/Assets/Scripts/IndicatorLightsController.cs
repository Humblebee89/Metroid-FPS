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
        Actions.OnBeamChange += BeamChange;
    }
    private void OnDisable()
    {
        Actions.OnChargeStarted -= ChargeStarted;
        Actions.OnChargeCooldownEnd -= ChargeCooldownEnd;
        Actions.OnBeamChange -= BeamChange;
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

    private void BeamChange()
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                lightMaterial.EnableKeyword("_ACTIVEBEAM_POWER");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_WAVE");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_ICE");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                lightMaterial.DisableKeyword("_ACTIVEBEAM_POWER");
                lightMaterial.EnableKeyword("_ACTIVEBEAM_WAVE");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_ICE");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                lightMaterial.DisableKeyword("_ACTIVEBEAM_POWER");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_WAVE");
                lightMaterial.EnableKeyword("_ACTIVEBEAM_ICE");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                lightMaterial.DisableKeyword("_ACTIVEBEAM_POWER");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_WAVE");
                lightMaterial.DisableKeyword("_ACTIVEBEAM_ICE");
                lightMaterial.EnableKeyword("_ACTIVEBEAM_PLASMA");
                break;
        }
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
