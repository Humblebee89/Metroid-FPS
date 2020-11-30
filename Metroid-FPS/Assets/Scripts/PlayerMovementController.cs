using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float JumpHeight = 1f;

    private Vector3 moveDirection;
    private Rigidbody playerRigidbody;
    private Vector2 inputDirection;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Move.performed += context => GetMoveInput(context.ReadValue<Vector2>());
        playerInput.Player.Move.canceled += context => GetMoveInput(context.ReadValue<Vector2>());
        playerInput.Player.Jump.performed += context => Jump();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void GetMoveInput(Vector2 input)
    {
        inputDirection = input;
    }

    private void Jump()
    {
        //TODO add grounded check so you can't jump infinitely
        playerRigidbody.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDirection * moveSpeed * Time.deltaTime);
    }
}
