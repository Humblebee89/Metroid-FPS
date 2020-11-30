using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float clampAngle = 90f;
  
    private float inputX;
    private float inputY;
    private float clampedRotationX;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Look.performed += context => GetLookInput(context.ReadValue<Vector2>());
        playerInput.Player.Look.canceled += context => GetLookInput(context.ReadValue<Vector2>());
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void GetLookInput(Vector2 axis)
    {
        inputX = axis.x;
        inputY = axis.y;
    }

    private void Update()
    {
        //Camera Look
        float modifiedInputX = inputX * lookSensitivity * Time.deltaTime;
        float modifiedInputY = inputY * lookSensitivity * Time.deltaTime;

        clampedRotationX -= modifiedInputY;
        clampedRotationX = Mathf.Clamp(clampedRotationX, -clampAngle, clampAngle);

        playerCamera.localRotation = Quaternion.Euler(clampedRotationX, 0f, 0f);
        playerBody.Rotate(Vector3.up * modifiedInputX);
    }

}
