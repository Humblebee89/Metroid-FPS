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
    [SerializeField] private AnimationCurve inputCurve;
    [SerializeField] private float clampAngle = 90f;
  
    private float inputX;
    private float inputY;
    private float clampedRotationX;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Look.performed += context => GetLookInput(context.ReadValue<Vector2>());
        playerInput.Player.Look.canceled += context => GetLookInput(context.ReadValue<Vector2>());

        inputCurve.preWrapMode = WrapMode.PingPong;
    }

    private void OnEnable()
    {
        playerInput.Enable();

        Cursor.visible = false;
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void GetLookInput(Vector2 axis)
    {
        print("axis " + axis);
        inputX = axis.x;
        inputY = axis.y;
    }

    private void Update()
    {
        //Camera Look
        float curveEvaluatedX = inputCurve.Evaluate(inputX) * inputX;
        float curveEvaluatedY = inputCurve.Evaluate(inputY) * inputY;

        float modifiedInputX = curveEvaluatedX * lookSensitivity * Time.deltaTime;
        float modifiedInputY = curveEvaluatedY * lookSensitivity * Time.deltaTime;

        clampedRotationX -= modifiedInputY;
        clampedRotationX = Mathf.Clamp(clampedRotationX, -clampAngle, clampAngle);

        playerCamera.localRotation = Quaternion.Euler(clampedRotationX, 0f, 0f);
        playerBody.Rotate(Vector3.up * modifiedInputX);
    }

}
