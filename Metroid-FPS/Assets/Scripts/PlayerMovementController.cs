using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField] private bool enableDoubleJumpAbility;
    [SerializeField] private bool enableDashAbility;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [HideInInspector] public Vector2 inputDirection;

    private Rigidbody playerRigidbody;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool canDash = true;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        playerInput.Player.Move.performed += context => GetMoveInput(context.ReadValue<Vector2>());
        playerInput.Player.Move.canceled += context => GetMoveInput(context.ReadValue<Vector2>());
        playerInput.Player.Jump.performed += context => Jump();
        playerInput.Player.Dash.performed += context => Dash();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void GetMoveInput(Vector2 input)
    {
        inputDirection = input;
    }

    private void Dash()
    {
        if(enableDashAbility && canDash)
        {
            StartCoroutine(PerformDash());
            StartCoroutine(CoolDownHelper.CoolDown(dashCooldown, value => canDash = value));
        }
    }

    private IEnumerator PerformDash()
    {
        playerRigidbody.velocity = Vector3.zero;
        float elapsedTime = 0;

        while (elapsedTime < dashTime)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection.normalized * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void Jump()
    {
        if(isGrounded)
        {
            PerformJump();
            canDoubleJump = true;
        }
        else if(canDoubleJump && enableDoubleJumpAbility)
        {
            PerformJump();    
            canDoubleJump = false;
        }
    }

    private void PerformJump()
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        playerRigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
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
