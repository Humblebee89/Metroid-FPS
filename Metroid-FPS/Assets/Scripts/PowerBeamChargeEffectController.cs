using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBeamChargeEffectController : MonoBehaviour
{
    public PlayerWeaponController playerWeaponController;
    public GameObject effectVisual;
    [SerializeField] private AnimationCurve sizeAdjustCurve;
    [SerializeField] private Light chargeLight;

    private float lightIntensity;

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
    }

    private void ChargeStarted()
    {
        print("Charge Started");
        StartCoroutine("Charge");
    }

    private IEnumerator Charge()
    {
        while (playerWeaponController.chargevalue < 1)
        {
            float adjustedScale = sizeAdjustCurve.Evaluate(playerWeaponController.chargevalue);
            float adjustedLightIntensity = Extensions.Remap(adjustedScale, 0, 1, 0, lightIntensity);
            chargeLight.intensity = adjustedLightIntensity;
            transform.localScale = new Vector3(adjustedScale, adjustedScale, adjustedScale);
            yield return null;
        } 
    }
}
