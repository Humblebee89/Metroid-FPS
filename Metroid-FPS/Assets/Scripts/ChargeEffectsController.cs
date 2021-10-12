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
        StartCoroutine("Charge");
    }

    private IEnumerator Charge()
    {
        while (playerWeaponController.chargevalue < 1)
        {
            float adjustedScale = sizeAdjustCurve.Evaluate(playerWeaponController.chargevalue);
            ChargeEffectGameObject.transform.localScale = new Vector3(adjustedScale, adjustedScale, adjustedScale);
            yield return null;
        } 
    }

    private void FireCharged()
    {
        ChargeEffectGameObject.SetActive(false);
    }
}
