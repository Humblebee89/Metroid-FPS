using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCannonEffectsController : MonoBehaviour
{
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material energyFieldMaterial;

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

    private void Start()
    {
        UpdateMaterial(lightMaterial);
        UpdateMaterial(energyFieldMaterial);
    }

    private void ChargeStarted()
    {
        charging = true;
    }

    private void ChargeCooldownEnd()
    {
        charging = false;

        if(playerWeaponController.activeBeam == PlayerWeaponController.ActiveBeam.Power)
            energyFieldMaterial.SetFloat("_Transparency", 0.0f);
    }

    private void BeamChange()
    {
        UpdateMaterial(lightMaterial);
        UpdateMaterial(energyFieldMaterial);
    }

    private void UpdateMaterial(Material material)
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                energyFieldMaterial.SetFloat("_Transparency", playerWeaponController.chargeValue);
                material.EnableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                energyFieldMaterial.SetFloat("_Transparency", 1.0f);
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.EnableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                energyFieldMaterial.SetFloat("_Transparency", 1.0f);
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.EnableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                energyFieldMaterial.SetFloat("_Transparency", 1.0f);
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.EnableKeyword("_ACTIVEBEAM_PLASMA");
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

        if (playerWeaponController.activeBeam == PlayerWeaponController.ActiveBeam.Power && charging)
        {
            energyFieldMaterial.SetFloat("_Transparency", playerWeaponController.chargeValue);
        }
    }
}
