using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffectsController : MonoBehaviour
{
    [SerializeField] PlayerWeaponController playerWeaponController;
    [SerializeField] GameObject PowerBeamChargeEffectGameObject;
    [SerializeField] GameObject WaveBeamChargeEffectGameObject;
    [SerializeField] private AnimationCurve sizeAdjustCurve;

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
                ChargeEffectGameObject = PowerBeamChargeEffectGameObject;
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                ChargeEffectGameObject = WaveBeamChargeEffectGameObject;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                //Add Effect
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                //Add Effect
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
