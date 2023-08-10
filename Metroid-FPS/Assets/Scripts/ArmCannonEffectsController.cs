using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCannonEffectsController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer glowMeshRenderer;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material powerEnergyFieldMaterial;
    [SerializeField] private Material waveEnergyFieldMaterial;
    [SerializeField] private Material iceEnergyFieldMaterial;
    [SerializeField] private Material plasmaEnergyFieldMaterial;
    [SerializeField] private ParticleSystem powerBeamParticleSystem;
    [SerializeField] private float powerBeamEmissionMultiplier;
    [SerializeField] private ParticleSystem waveBeamParticleSystem;
    [SerializeField] private float waveBeamEmissionMultiplier;
    [SerializeField] private ParticleSystem iceBeamParticleSystem;
    [SerializeField] private float iceBeamEmissionMultiplier;
    [SerializeField] private ParticleSystem plasmaBeamParticleSystem;
    [SerializeField] private float plasmaBeamEmissionMultiplier;

    private PlayerWeaponController playerWeaponController;
    private ParticleSystem activeParticleSystem;
    private bool charging;
    private ParticleSystem.EmissionModule waveBeamEmission;

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

    private void Start()
    {
        waveBeamEmission = waveBeamParticleSystem.emission;

        UpdateLightMaterial(lightMaterial);
        UpdateGlowMaterial();
        ChangeParticleEffect();
    }

    private void ChargeStarted()
    {
        charging = true;
    }

    private void ChargeCooldownEnd()
    {
        charging = false;

        if(playerWeaponController.activeBeam == PlayerWeaponController.ActiveBeam.Power)
            powerEnergyFieldMaterial.SetFloat("_Transparency", 0.0f);
    }

    public void BeamChange()
    {
        UpdateLightMaterial(lightMaterial);
        UpdateGlowMaterial();
        ChangeParticleEffect();
    }

    private void UpdateLightMaterial(Material material)
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                powerEnergyFieldMaterial.SetFloat("_Transparency", playerWeaponController.chargeValue);
                material.EnableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.EnableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.EnableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.EnableKeyword("_ACTIVEBEAM_PLASMA");
                break;
        }
    }

    private void UpdateGlowMaterial()
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                glowMeshRenderer.material = powerEnergyFieldMaterial;
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                glowMeshRenderer.material = waveEnergyFieldMaterial;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                glowMeshRenderer.material = iceEnergyFieldMaterial;
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                glowMeshRenderer.material = plasmaEnergyFieldMaterial;
                break;
        }
    }

    private void ChangeParticleEffect()
    {
        //powerBeamParticleSystem.gameObject.SetActive(false);
        waveBeamParticleSystem.gameObject.SetActive(false);
        //iceBeamParticleSystem.gameObject.SetActive(false);
        //plasmaBeamParticleSystem.gameObject.SetActive(false);

        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                //powerBeamParticleSystem.gameObject.SetActive(true);
                activeParticleSystem = powerBeamParticleSystem;
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                waveBeamParticleSystem.gameObject.SetActive(true);
                activeParticleSystem = waveBeamParticleSystem;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                //iceBeamParticleSystem.gameObject.SetActive(true);
                activeParticleSystem = iceBeamParticleSystem;
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                //plasmaBeamParticleSystem.gameObject.SetActive(true);
                activeParticleSystem = plasmaBeamParticleSystem;
                break;
        }
    }

    private void UpdateParticleEffect()
    {


        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                //Add Once Effect is created
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                waveBeamEmission.rateOverTimeMultiplier = playerWeaponController.chargeValue * waveBeamEmissionMultiplier;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                //Add Once Effect is created
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                //Add Once Effect is created
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
            UpdateParticleEffect();
        }

        if (playerWeaponController.activeBeam == PlayerWeaponController.ActiveBeam.Power && charging)
        {
            powerEnergyFieldMaterial.SetFloat("_Transparency", playerWeaponController.chargeValue);
        }
    }
}
