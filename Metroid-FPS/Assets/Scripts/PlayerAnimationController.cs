using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private Animator ArmCannonAnimator;

    private float playerVelocityMagnitude;
    private Vector3 previous;


    private void Update()
    {
        GetPlayerVelocity();
        print(playerVelocityMagnitude);
        ArmCannonAnimator.SetFloat("WalkSpeed", playerVelocityMagnitude);
    }

    private void GetPlayerVelocity()
    {
        playerVelocityMagnitude = (playerMovementController.inputDirection).magnitude;
        //previous = transform.position;
    }

}
