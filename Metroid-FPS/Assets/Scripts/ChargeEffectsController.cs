using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffectsController : MonoBehaviour
{
    [SerializeField] PlayerWeaponController playerWeaponController;
    [SerializeField] Transform projectileSource;
    [SerializeField] GameObject powerBeamChargeEffect;
    [SerializeField] private AnimationCurve sizeAdjustCurve;

    private GameObject instantiatedChargeEffect;

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
        instantiatedChargeEffect = Instantiate(powerBeamChargeEffect, projectileSource.position, Quaternion.identity, projectileSource);
        instantiatedChargeEffect.SetActive(false);
    }

    private void FireNormal()
    {
        instantiatedChargeEffect.SetActive(false);
    }

    private void ChargeStarted()
    {
        instantiatedChargeEffect.SetActive(true);
        StartCoroutine("Charge");
    }

    private IEnumerator Charge()
    {
        while (playerWeaponController.chargevalue < 1)
        {
            float adjustedScale = sizeAdjustCurve.Evaluate(playerWeaponController.chargevalue);
            instantiatedChargeEffect.transform.localScale = new Vector3(adjustedScale, adjustedScale, adjustedScale);
            yield return null;
        } 
    }

    private void FireCharged()
    {
        instantiatedChargeEffect.SetActive(false);
    }
}
