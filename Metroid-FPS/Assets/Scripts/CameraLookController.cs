using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    public PlayerInput playerInput;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerCamera;
    
    private float inputX;
    private float inputY;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Look.performed += context => Look(context.ReadValue<Vector2>());
        playerInput.Player.Look.canceled += context => Look(context.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Look(Vector2 axis)
    {
        inputX = axis.x;
        inputY = axis.y;
    }

    private void Update()
    {
        //Camera Look
        //TODO Clamp Camera rotation
        playerBody.Rotate(Vector3.up * inputX * lookSensitivity * Time.deltaTime);
        playerCamera.Rotate(Vector3.left * inputY * lookSensitivity * Time.deltaTime);
    }

}
