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
    [HideInInspector] public bool isGrounded;

    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;
    private Vector3 moveDirection;
    //private bool canDoubleJump;
    private int jumpAmount = 1;
    private bool canDash = true;
    private bool deltaIsGrounded;
    private float dynamicFriction;
    private float staticFriction;
    private PhysicMaterialCombine physicMaterialCombine;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerInput = new PlayerInput();
        playerInput.Player.Move.performed += context => GetMoveInput(context.ReadValue<Vector2>());
        playerInput.Player.Move.canceled += context => GetMoveInput(context.ReadValue<Vector2>());
        playerInput.Player.Jump.performed += context => Jump();
        playerInput.Player.Dash.performed += context => Dash();

        dynamicFriction = playerCollider.material.dynamicFriction;
        staticFriction = playerCollider.material.staticFriction;
        physicMaterialCombine = playerCollider.material.frictionCombine;
    }

    private void OnEnable()
    {
        playerInput.Enable();

        Actions.OnGrounded += OnGrounded;
        Actions.OnAirBorne += OnAirborne;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        Actions.OnGrounded -= OnGrounded;
        Actions.OnAirBorne -= OnAirborne;
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
        }

        if(enableDoubleJumpAbility && isGrounded == false  && jumpAmount != 0)
        {
            PerformJump();
            jumpAmount--;
        }
    }

    private void PerformJump()
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        playerRigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    private void OnAirborne()
    {
        playerCollider.material.dynamicFriction = 0;
        playerCollider.material.staticFriction = 0;
        playerCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
    }

    private void OnGrounded()
    {
        playerCollider.material.dynamicFriction = dynamicFriction;
        playerCollider.material.staticFriction = staticFriction;
        playerCollider.material.frictionCombine = physicMaterialCombine;

        jumpAmount = 1;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if( isGrounded != deltaIsGrounded)
        {
            if (isGrounded)
                Actions.OnGrounded();
            else
                Actions.OnAirBorne();
        }

        deltaIsGrounded = isGrounded;

        moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDirection * moveSpeed * Time.deltaTime);

    }
}
