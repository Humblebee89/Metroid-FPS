using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetAnimationController : MonoBehaviour
{
    [SerializeField] private CameraLookController cameraLookController;
    [SerializeField] private float magnitude = 0.5f;
    [SerializeField] private float speed = 0.1f;

    private Vector2 inputDirection;
    private Quaternion targetRotation;
    private float timeCount = 0f;

    private void Update()
    {
        inputDirection = new Vector2(-cameraLookController.inputDirection.x, cameraLookController.inputDirection.y * magnitude);
        targetRotation = Quaternion.Euler(inputDirection.y, inputDirection.x, transform.rotation.z);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
    }
}
