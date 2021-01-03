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
    [SerializeField] bool enableDoubleJump;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    private Vector3 moveDirection;
    private Rigidbody playerRigidbody;
    private Vector2 inputDirection;
    private bool isGrounded;
    private bool canDoubleJump;

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
        if(isGrounded)
        {
            PerformJump();
            canDoubleJump = true;
        }

        if(isGrounded == false && canDoubleJump && enableDoubleJump)
        {
            PerformJump();    
            canDoubleJump = false;
        }

    }

    private void PerformJump()
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        playerRigidbody.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void FixedUpdate()
    {
        moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDirection * moveSpeed * Time.deltaTime);
    }
}
