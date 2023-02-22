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
    private bool barrelOpen;

    private void OnEnable()
    {
        Actions.OnFireNormal += FireNormal;
        Actions.OnFireCharged += FireCharged;
        Actions.OnFireMissile += FireMissile;
        Actions.OnBeamChange += BeamChange;
    }

    private void OnDisable()
    {
        Actions.OnFireNormal -= FireNormal;
        Actions.OnFireCharged -= FireCharged;
        Actions.OnFireMissile -= FireMissile;
        Actions.OnBeamChange -= BeamChange;
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

    private void BeamChange()
    {
        if (barrelOpen)
        {
            armCannonAnimator.SetTrigger("MissileClose");
            barrelOpen = false;
        }

        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                armCannonAnimator.SetTrigger("PowerBeamSwap");
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                armCannonAnimator.SetTrigger("WaveBeamSwap");
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                //Add Animation
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                //Add Animation
                break;
        }
    }

    private void FireNormal()
    {
        if (barrelOpen)
        {
            armCannonAnimator.SetTrigger("MissileClose");
            barrelOpen = false;
            return;
        }
        else
            armCannonAnimator.SetTrigger("FireNormal");
    }

    private void FireCharged()
    {
        armCannonAnimator.SetTrigger("FireCharged");
    }

    private void FireMissile()
    {
        armCannonAnimator.SetTrigger("MissileOpen");
        armCannonAnimator.SetTrigger("FireCharged");
        barrelOpen = true;
    }
}
