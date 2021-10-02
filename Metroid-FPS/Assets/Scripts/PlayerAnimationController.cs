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
    }
    
    private void OnDisable()
    {
        Actions.OnFireNormal -= FireNormal;
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

}
