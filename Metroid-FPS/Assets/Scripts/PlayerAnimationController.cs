using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private Animator ArmCannonAnimator;

    private float playerVelocityMagnitude;

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
        ArmCannonAnimator.SetFloat("WalkSpeed", playerVelocityMagnitude);
        ArmCannonAnimator.SetBool("isGrounded", playerMovementController.isGrounded);
        ArmCannonAnimator.SetLayerWeight(1, playerWeaponController.chargevalue);
    }

    private void GetPlayerVelocity()
    {
        playerVelocityMagnitude = (playerMovementController.inputDirection).magnitude;
    }

    private void FireNormal()
    {
        ArmCannonAnimator.SetTrigger("FireNormal");
    }

    private void FireCharged()
    {
        ArmCannonAnimator.SetTrigger("FireCharged");
    }

}
