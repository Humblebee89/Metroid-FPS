using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private Animator armCannonAnimator;
    [SerializeField] private AnimationCurve shakeAdjustCurve;

    private float playerVelocityMagnitude;
    private float adjustedShakeAmount;

    private void OnEnable()
    {
        Actions.OnFireNormal += FireNormal;
        Actions.OnFireCharged += FireCharged;
    }

    private void OnDisable()
    {
        Actions.OnFireNormal -= FireNormal;
        Actions.OnFireCharged -= FireCharged;
    }

    private void Update()
    {
        GetPlayerVelocity();
        Shake();
        armCannonAnimator.SetFloat("WalkSpeed", playerVelocityMagnitude);
        armCannonAnimator.SetBool("isGrounded", playerMovementController.isGrounded);
        armCannonAnimator.SetFloat("ChargeValue", playerWeaponController.chargevalue);
    }

    private void GetPlayerVelocity()
    {
        playerVelocityMagnitude = (playerMovementController.inputDirection).magnitude;
    }

    private void Shake()
    {
        adjustedShakeAmount = shakeAdjustCurve.Evaluate(playerWeaponController.chargevalue);
        armCannonAnimator.SetLayerWeight(1, adjustedShakeAmount);
    }

    private void FireNormal()
    {
        armCannonAnimator.SetTrigger("FireNormal");
    }

    private void FireCharged()
    {
        armCannonAnimator.SetTrigger("FireCharged");
    }

}
