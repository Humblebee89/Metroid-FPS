using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private Animator armCannonAnimator;
    [SerializeField] private AnimationCurve shakeAdjustCurve;
    [SerializeField] private float acceloratorMultiplierHighBound;
    [SerializeField] private float acceloratorMultiplierLowBound;
    [SerializeField] private float acceloratorMultiplier;
    [SerializeField] private float acceleratorDecayRate;
    [SerializeField] private float powerBeamAcceloratorAddValue;
    [SerializeField] private float waveBeamAcceloratorAddValue;
    [SerializeField] private float iceBeamAcceloratorAddValue;
    [SerializeField] private float plasmaBeamAcceloratorAddValue;

    private float playerVelocityMagnitude;
    private float adjustedShakeAmount;
    private bool barrelOpen;

    private void OnEnable()
    {
        Actions.OnFireNormal += FireNormal;
        Actions.OnFireCharged += FireCharged;
        Actions.OnChargeStarted += ChargeStarted;
        Actions.OnFireMissile += FireMissile;
        Actions.OnBeamChange += BeamChange;
    }

    private void OnDisable()
    {
        Actions.OnFireNormal -= FireNormal;
        Actions.OnFireCharged -= FireCharged;
        Actions.OnChargeStarted -= ChargeStarted;
        Actions.OnFireMissile -= FireMissile;
        Actions.OnBeamChange -= BeamChange;
    }

    private void Update()
    {
        GetPlayerVelocity();
        Shake();
        AcceloratorDecay();
        armCannonAnimator.SetFloat("WalkSpeed", playerVelocityMagnitude);
        armCannonAnimator.SetBool("isGrounded", playerMovementController.isGrounded);
        armCannonAnimator.SetFloat("ChargeValue", playerWeaponController.chargeValue);
    }

    private void GetPlayerVelocity()
    {
        playerVelocityMagnitude = (playerMovementController.inputDirection).magnitude;
    }

    private void Shake()
    {
            adjustedShakeAmount = shakeAdjustCurve.Evaluate(playerWeaponController.chargeValue);
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
                armCannonAnimator.SetTrigger("BeamSwap");
                armCannonAnimator.SetInteger("ActiveBeam", 0);
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                armCannonAnimator.SetTrigger("BeamSwap");
                armCannonAnimator.SetInteger("ActiveBeam", 1);
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
        {
            armCannonAnimator.SetTrigger("FireNormal");
            AddValueToAccelerator(1);
        }

    }

    private void ChargeStarted()
    {
        if (barrelOpen)
        {
            armCannonAnimator.SetTrigger("MissileClose");
            barrelOpen = false;
            return;
        }
    }

    private void FireCharged()
    {
        armCannonAnimator.SetTrigger("FireCharged");
        AddValueToAccelerator(3);
    }

    private void FireMissile()
    {
        armCannonAnimator.SetTrigger("MissileOpen");
        armCannonAnimator.SetTrigger("FireCharged");
        AddValueToAccelerator(5);
        barrelOpen = true;
    }

    private void AddValueToAccelerator(float multiplier)
    {
        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                armCannonAnimator.SetFloat("AcceleratorMultiplier", acceloratorMultiplier += powerBeamAcceloratorAddValue * multiplier);
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                armCannonAnimator.SetFloat("AcceleratorMultiplier", acceloratorMultiplier += waveBeamAcceloratorAddValue * multiplier);
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                armCannonAnimator.SetFloat("AcceleratorMultiplier", acceloratorMultiplier += iceBeamAcceloratorAddValue * multiplier);
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                armCannonAnimator.SetFloat("AcceleratorMultiplier", acceloratorMultiplier += plasmaBeamAcceloratorAddValue * multiplier);
                break;
        }

    }

    private void AcceloratorDecay()
    {
        if (acceloratorMultiplier == acceloratorMultiplierLowBound)
            return;

        if (acceloratorMultiplier < acceloratorMultiplierLowBound)
        {
            acceloratorMultiplier = acceloratorMultiplierLowBound;
            return;
        }

        if (acceloratorMultiplier > acceloratorMultiplierHighBound)
            acceloratorMultiplier = acceloratorMultiplierHighBound;

        acceloratorMultiplier -= acceleratorDecayRate * Time.deltaTime;

        armCannonAnimator.SetFloat("AcceleratorMultiplier", acceloratorMultiplier);
    }
}
