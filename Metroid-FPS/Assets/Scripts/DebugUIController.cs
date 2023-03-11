using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUIController : MonoBehaviour
{
    [SerializeField] PlayerWeaponController playerWeaponController;
    [SerializeField] private TMP_Text currentBeamText;
    [SerializeField] private TMP_Text canFireText;
    [SerializeField] private TMP_Text chargeAmountText;


    private void OnEnable()
    {
        Actions.OnBeamChange += BeamChange;
        currentBeamText.text = "Active Beam: <color=#FFB731> Power";
    }

    private void OnDisable()
    {
        Actions.OnBeamChange -= BeamChange;
    }

    private void BeamChange()
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                currentBeamText.text = "Active Beam: <color=#FFB731> Power";
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                currentBeamText.text = "Active Beam: <color=#611AFF> Wave";
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                currentBeamText.text = "Active Beam: <color=#19E2FF> Ice";
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                currentBeamText.text = "Active Beam: <color=#D92900> Plasma";
                break;
        }
    }

    private void Update()
    {
        chargeAmountText.text = "Charge Amount: " + playerWeaponController.chargeValue;

        if (playerWeaponController.canFireNormalShot)
            canFireText.text = "Can Fire Normal: True";
        else
            canFireText.text = "Can Fire Normal: False";
    }

}
