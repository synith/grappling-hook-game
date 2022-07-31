using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameEventSO onPlayerPaused;

    [SerializeField]
    private float
        playerSpeed,
        jumpHeight,
        rotationSpeed,
        forceModifier,
        maxSpeed,
        maxSpeedAir,
        groundDistance;

    [SerializeField]
    private LayerMask jumpLayer;



    private bool isPlayerGrounded;

    private Vector3 moveDirection;

    private GrapplingHook hook;

    private Rigidbody playerRigidbody;
    private Transform cameraTransform;
    private Transform groundChecker;

    private PlayerInput playerInput;

    private InputAction
        moveAction,
        jumpAction,
        grappleAction,
        pauseAction;

    private Animator playerAnimator;

    private int isWalkingForwardHash;
    private int isWalkingBackwardsHash;
    private int isWalkingRightHash;
    private int isWalkingLeftHash;

    private const float DEAD_ZONE = 0.05f;


    private void Awake()
    {
        cameraTransform = Camera.main.transform;

        hook = GetComponent<GrapplingHook>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        groundChecker = transform.Find("groundChecker");
        playerAnimator = transform.Find("playerModel").GetComponent<Animator>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        grappleAction = playerInput.actions["Grapple"];
        pauseAction = playerInput.actions["Pause"];

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        isWalkingForwardHash = Animator.StringToHash("isWalkingForward");
        isWalkingBackwardsHash = Animator.StringToHash("isWalkingBackwards");
        isWalkingRightHash = Animator.StringToHash("isWalkingRight");
        isWalkingLeftHash = Animator.StringToHash("isWalkingLeft");
        Time.timeScale = 1f;
    }


    private void OnEnable()
    {
        jumpAction.performed += _ => OnJump();
        grappleAction.started += _ => hook.StartGrapple();
        pauseAction.performed += (context) => OnPlayerPaused(context);
    }


    private void OnDisable()
    {
        jumpAction.performed -= _ => OnJump();
        grappleAction.started -= _ => hook.StartGrapple();
        pauseAction.performed -= (context) => OnPlayerPaused(context);
    }


    private void Update()
    {
        bool isWalkingForward = playerAnimator.GetBool(isWalkingForwardHash);
        bool pressedForward = moveAction.ReadValue<Vector2>().y > DEAD_ZONE;

        bool isWalkingBackwards = playerAnimator.GetBool(isWalkingBackwardsHash);
        bool pressedBackwards = moveAction.ReadValue<Vector2>().y < -DEAD_ZONE;


        bool isWalkingRight = playerAnimator.GetBool(isWalkingRightHash);
        bool pressedRight = moveAction.ReadValue<Vector2>().x > DEAD_ZONE;

        bool isWalkingLeft = playerAnimator.GetBool(isWalkingLeftHash);
        bool pressedLeft = moveAction.ReadValue<Vector2>().x < -DEAD_ZONE;



        if (!isWalkingForward && pressedForward && isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingForwardHash, true);
        }
        if (isWalkingForward && !pressedForward || !isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingForwardHash, false);
        }


        if (!isWalkingBackwards && pressedBackwards && isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingBackwardsHash, true);
        }
        if (isWalkingBackwards && !pressedBackwards || !isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingBackwardsHash, false);
        }


        if (!isWalkingRight && pressedRight && isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingRightHash, true);
        }
        if (isWalkingRight && !pressedRight || !isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingRightHash, false);
        }

        if (!isWalkingLeft && pressedLeft && isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingLeftHash, true);
        }
        if (isWalkingLeft && !pressedLeft || !isPlayerGrounded)
        {
            playerAnimator.SetBool(isWalkingLeftHash, false);
        }

        CheckIfGrapplingStopped();
        RotatePlayerTowardsCamera();
        SetMovementDirectionFromInputAndCamera();


        void CheckIfGrapplingStopped()
        {
            if (grappleAction.WasReleasedThisFrame())
            {
                hook.StopGrapple();
            }
        }

        void SetMovementDirectionFromInputAndCamera()
        {
            moveDirection = PlayerInputVectorNormalized();
            moveDirection = MovementDirectionRelativeToCamera(moveDirection, cameraTransform);
            moveDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up).normalized;

            Vector3 PlayerInputVectorNormalized()
            {
                Vector2 input = moveAction.ReadValue<Vector2>();
                Vector3 move = new(input.x, 0, input.y);
                return move.normalized;
            }
            Vector3 MovementDirectionRelativeToCamera(Vector3 move, Transform cameraTransform)
            {
                move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
                move.y = 0f;
                return move;
            }
        }

        void RotatePlayerTowardsCamera()
        {
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void FixedUpdate()
    {
        isPlayerGrounded = CheckIfPlayerGrounded();
        MovePlayer();

        void MovePlayer()
        {
            playerRigidbody.drag = isPlayerGrounded ? 10f : 0.1f;

            float airModifier = isPlayerGrounded ? 1f : 0.5f;
            float maxSpeed = isPlayerGrounded ? this.maxSpeed : maxSpeedAir;

            moveDirection *= playerSpeed * airModifier * Time.fixedDeltaTime;

            Vector2 rigidbodyVelocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);

            float force = isPlayerGrounded ? forceModifier : forceModifier * 0.2f;

            if (rigidbodyVelocity.magnitude < maxSpeed)
            {
                playerRigidbody.AddForce(force * moveDirection);
            }
        }
    }


    private bool CheckIfPlayerGrounded() => Physics.CheckSphere(groundChecker.position, groundDistance, jumpLayer, QueryTriggerInteraction.Ignore);


    private void OnJump()
    {
        if (isPlayerGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            SoundManager.Instance.PlaySound(SoundManager.Sound.Jump);
            playerAnimator.SetTrigger("jumpTrigger");
        }
    }

    private void OnPlayerPaused(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPlayerPaused.TriggerEvent();
        }
    }
}
