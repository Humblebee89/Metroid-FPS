using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffectsController : MonoBehaviour
{
    [SerializeField] PlayerWeaponController playerWeaponController;
    [SerializeField] GameObject powerBeamChargeEffectGameObject;
    [SerializeField] GameObject waveBeamChargeEffectGameObject;
    [SerializeField] GameObject iceBeamChargeEffectGameObject;
    [SerializeField] GameObject plasmaBeamChargeEffectGameObject;

    private GameObject ChargeEffectGameObject;

    private void OnEnable()
    {
        Actions.OnFireNormal += FireNormal;
        Actions.OnChargeStarted += ChargeStarted;
        Actions.OnFireCharged += FireCharged;
        Actions.OnBeamChange += BeamChange;
    }

    private void OnDisable()
    {
        Actions.OnFireNormal -= FireNormal;
        Actions.OnChargeStarted -= ChargeStarted;
        Actions.OnFireCharged -= FireCharged;
    }

    private void Awake()
    {
        BeamChange();
        ChargeEffectGameObject.SetActive(false);
    }

    private void BeamChange()
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                ChargeEffectGameObject = powerBeamChargeEffectGameObject;
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                ChargeEffectGameObject = waveBeamChargeEffectGameObject;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                ChargeEffectGameObject = iceBeamChargeEffectGameObject;
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                ChargeEffectGameObject = plasmaBeamChargeEffectGameObject;
                break;
        }
    }

    private void FireNormal()
    {
        ChargeEffectGameObject.SetActive(false);
    }

    private void ChargeStarted()
    {
        ChargeEffectGameObject.SetActive(true);
    }

    private void FireCharged()
    {
        ChargeEffectGameObject.SetActive(false);
    }
}
