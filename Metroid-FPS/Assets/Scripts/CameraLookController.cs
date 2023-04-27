using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    public PlayerInput playerInput;
    [HideInInspector]
    public Vector2 inputDirection;

    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private AnimationCurve inputCurve;
    [SerializeField] private float clampAngle = 90f;

    
    private float clampedRotationX;
    private float modifiedInputX;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Look.performed += context => GetLookInput(context.ReadValue<Vector2>());
        playerInput.Player.Look.canceled += context => GetLookInput(context.ReadValue<Vector2>());

        inputCurve.preWrapMode = WrapMode.PingPong;

        modifiedInputX = playerBody.transform.eulerAngles.y;
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

    private void Update()
    {
        CameraLook();
    }

    private void GetLookInput(Vector2 axis)
    {
        Vector2 dir = axis.normalized;
        inputDirection = axis / Mathf.Max (Mathf.Abs (dir.x), Mathf.Abs (dir.y), Mathf.Epsilon);
    }

    private void CameraLook()
    {
        int xDirection;
        int yDirection;

        xDirection = inputDirection.x > 0 ? 1 : -1;
        yDirection = inputDirection.y > 0 ? 1 : -1;

        float curveEvaluatedX = inputCurve.Evaluate(inputDirection.x) * xDirection;
        float curveEvaluatedY = inputCurve.Evaluate(inputDirection.y) * yDirection;

        modifiedInputX = modifiedInputX + curveEvaluatedX * lookSensitivity * Time.deltaTime % 360f;
        float modifiedInputY = curveEvaluatedY * lookSensitivity * Time.deltaTime;

        clampedRotationX -= modifiedInputY;
        clampedRotationX = Mathf.Clamp(clampedRotationX, -clampAngle, clampAngle);

        playerCamera.localRotation = Quaternion.Euler(clampedRotationX, 0f, 0f);
        playerBody.MoveRotation(Quaternion.Euler(0f, modifiedInputX, 0f));
    }
}
