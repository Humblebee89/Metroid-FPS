using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetAnimationController : MonoBehaviour
{
    [SerializeField] private CameraLookController cameraLookController;

    private Vector2 inputDirection;

    private void Update()
    {
        inputDirection = new Vector2(-cameraLookController.inputDirection.x, cameraLookController.inputDirection.y * 0.5f);
        transform.localRotation = Quaternion.Euler(inputDirection.y, inputDirection.x, transform.rotation.z);
    }
}
