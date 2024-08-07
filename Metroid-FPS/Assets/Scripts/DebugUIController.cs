using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUIController : MonoBehaviour
{
    [SerializeField] PlayerWeaponController playerWeaponController;
    [SerializeField] PlayerMovementController playerMovementController;
    [SerializeField] private TMP_Text currentBeamText;
    [SerializeField] private TMP_Text canFireText;
    [SerializeField] private TMP_Text chargeAmountText;
    [SerializeField] private TMP_Text groundedText;
 

    private void OnEnable()
    {
        Actions.OnBeamChange += BeamChange;
        Actions.OnGrounded += Grounded;
        Actions.OnAirBorne += Grounded;
        currentBeamText.text = "Active Beam: <color=#FFB731> Power";
    }

    private void OnDisable()
    {
        Actions.OnBeamChange -= BeamChange;
        Actions.OnGrounded -= Grounded;
        Actions.OnAirBorne -= Grounded;
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

    private void Grounded()
    {
        groundedText.text = "Grounded " + playerMovementController.isGrounded.ToString();
    }

    private void Update()
    {
        chargeAmountText.text = "Charge Amount: " + playerWeaponController.chargeValue;

        if (playerWeaponController.canFire)
            canFireText.text = "Can Fire Normal: True";
        else
            canFireText.text = "Can Fire Normal: False";
    }

}
