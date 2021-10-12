using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffectsController : MonoBehaviour
{
    [SerializeField] PlayerWeaponController playerWeaponController;
    [SerializeField] GameObject ChargeEffectGameObject;
    [SerializeField] private AnimationCurve sizeAdjustCurve;

    private void OnEnable()
    {
        Actions.OnFireNormal += FireNormal;
        Actions.OnChargeStarted += ChargeStarted;
        Actions.OnFireCharged += FireCharged;
    }

    private void OnDisable()
    {
        Actions.OnFireNormal -= FireNormal;
        Actions.OnChargeStarted -= ChargeStarted;
        Actions.OnFireCharged -= FireCharged;
    }

    private void Awake()
    {
        ChargeEffectGameObject.SetActive(false);
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
