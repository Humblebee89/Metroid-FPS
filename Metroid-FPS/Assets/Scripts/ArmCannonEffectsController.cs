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
    [SerializeField] private ParticleSystem snowflakeParticleSystem;
    [SerializeField] private float snowflakeEmissionMultiplier;
    [SerializeField] private ParticleSystem plasmaBeamParticleSystem;
    [SerializeField] private float plasmaBeamEmissionMultiplier;
    [SerializeField] private GameObject heatDistortionGameObject;

    private PlayerWeaponController playerWeaponController;
    private bool charging;
    private ParticleSystem.EmissionModule[] beamEmission;
    private float initialIcebeamEmissionRate;
    private float initialSnowflakeEmissionRate;
    private float initialPlasmaAshEmissionRate;
    private Animator heatDistortionAnimator;

    private void OnEnable()
    {
        Actions.OnChargeStarted += ChargeStarted;
        Actions.OnChargeCooldownEnd += ChargeCooldownEnd;
        Actions.OnBeamChange += BeamSwap;
    }
    private void OnDisable()
    {
        Actions.OnChargeStarted -= ChargeStarted;
        Actions.OnChargeCooldownEnd -= ChargeCooldownEnd;
        Actions.OnBeamChange -= BeamSwap;
    }

    private void Awake()
    {
        playerWeaponController = GetComponent<PlayerWeaponController>();
        heatDistortionAnimator = heatDistortionGameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        //change this if adding more particle systems
        beamEmission = new ParticleSystem.EmissionModule[2];
        initialIcebeamEmissionRate = iceBeamParticleSystem.emission.rateOverTimeMultiplier;
        initialSnowflakeEmissionRate = snowflakeParticleSystem.emission.rateOverTimeMultiplier;
        initialPlasmaAshEmissionRate = plasmaBeamParticleSystem.emission.rateOverTimeMultiplier;

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

    public void BeamChange() //This gets called from the AnimationEventPasser durring the beam transition animation
    {
        UpdateLightMaterial(lightMaterial);
        UpdateGlowMaterial();
        ChangeParticleEffect();
    }

    private void BeamSwap()
    {
        heatDistortionAnimator.SetTrigger("BeamSwap");
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
                CancelInvoke("UpdateWaveBeamEnergyField");
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.EnableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                InvokeRepeating("UpdateWaveBeamEnergyField", 0.1f, 0.1f);
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.EnableKeyword("_ACTIVEBEAM_ICE");
                material.DisableKeyword("_ACTIVEBEAM_PLASMA");
                CancelInvoke("UpdateWaveBeamEnergyField");
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                material.DisableKeyword("_ACTIVEBEAM_POWER");
                material.DisableKeyword("_ACTIVEBEAM_WAVE");
                material.DisableKeyword("_ACTIVEBEAM_ICE");
                material.EnableKeyword("_ACTIVEBEAM_PLASMA");
                CancelInvoke("UpdateWaveBeamEnergyField");
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
        iceBeamParticleSystem.gameObject.SetActive(false);
        snowflakeParticleSystem.gameObject.SetActive(false);
        plasmaBeamParticleSystem.gameObject.SetActive(false);
        heatDistortionGameObject.gameObject.SetActive(false);

        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                //powerBeamParticleSystem.gameObject.SetActive(true);
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                waveBeamParticleSystem.gameObject.SetActive(true);
                beamEmission[0] = waveBeamParticleSystem.emission;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                iceBeamParticleSystem.gameObject.SetActive(true);
                snowflakeParticleSystem.gameObject.SetActive(true);
                beamEmission[0] = iceBeamParticleSystem.emission;
                beamEmission[1] = snowflakeParticleSystem.emission;
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                plasmaBeamParticleSystem.gameObject.SetActive(true);
                heatDistortionGameObject.SetActive(true);
                beamEmission[0] = plasmaBeamParticleSystem.emission;
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
                beamEmission[0].rateOverTimeMultiplier = playerWeaponController.chargeValue * waveBeamEmissionMultiplier;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                beamEmission[0].rateOverTimeMultiplier = playerWeaponController.chargeValue * iceBeamEmissionMultiplier + initialIcebeamEmissionRate;
                beamEmission[1].rateOverTimeMultiplier = playerWeaponController.chargeValue * snowflakeEmissionMultiplier + initialSnowflakeEmissionRate;
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                beamEmission[0].rateOverTimeMultiplier = playerWeaponController.chargeValue * plasmaBeamEmissionMultiplier + initialPlasmaAshEmissionRate;
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

    private void UpdateWaveBeamEnergyField()
    {
        var position = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        waveEnergyFieldMaterial.SetVector("_Position", position);
    }
}
