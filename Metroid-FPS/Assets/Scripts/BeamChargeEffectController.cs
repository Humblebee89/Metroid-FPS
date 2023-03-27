using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamChargeEffectController : MonoBehaviour
{
    public PlayerWeaponController playerWeaponController;
    [SerializeField] private AnimationCurve sizeAdjustCurve;
    [SerializeField] private Light chargeLight;

    private float lightIntensity;
    private float lightRange;

    private void OnEnable()
    {
        Actions.OnChargeStarted += ChargeStarted;
    }

    private void OnDisable()
    {
        Actions.OnChargeStarted -= ChargeStarted;
    }

    private void Awake()
    {
        lightIntensity = chargeLight.intensity;
        lightRange = chargeLight.range;
    }

    private void ChargeStarted()
    {
        print("Charge Started");
        StartCoroutine("Charge");
    }

    private IEnumerator Charge()
    {
        while (playerWeaponController.chargeValue < 1)
        {
            float adjustedScale = sizeAdjustCurve.Evaluate(playerWeaponController.chargeValue);
            float adjustedLightIntensity = Extensions.Remap(adjustedScale, 0, 1, 0, lightIntensity);
            float adjustedLightRange = Extensions.Remap(adjustedScale, 0, 1, 0, lightRange);
            chargeLight.intensity = adjustedLightIntensity;
            transform.localScale = new Vector3(adjustedScale, adjustedScale, adjustedScale);
            yield return null;
        } 
    }
}
