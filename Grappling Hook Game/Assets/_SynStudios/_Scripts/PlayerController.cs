using System;
using System.Collections;
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

    private bool playerGrounded;
    private bool playerJumpedRecently;

    private Vector3 moveDirection;

    private GrapplingHook hook;

    private Rigidbody playerRigidbody;
    private Transform cameraTransform;
    private Transform groundChecker;

    private PlayerControls playerControls;

    private InputAction
        moveAction,
        jumpAction,
        grappleAction,
        pauseAction;

    private Animator playerAnimator;

    private int
        isWalkingForwardHash,
        isWalkingBackwardsHash,
        isWalkingRightHash,
        isWalkingLeftHash;

    private const float DEAD_ZONE = 0.05f;


    private void Awake()
    {
        cameraTransform = Camera.main.transform;

        playerControls = new PlayerControls();

        hook = GetComponent<GrapplingHook>();
        playerRigidbody = GetComponent<Rigidbody>();

        groundChecker = transform.Find("groundChecker");
        playerAnimator = transform.Find("playerModel").GetComponent<Animator>();

        moveAction = playerControls.Player.Move;
        jumpAction = playerControls.Player.Jump;
        grappleAction = playerControls.Player.Grapple;
        pauseAction = playerControls.Player.Pause;
    }

    private void Start()
    {
        isWalkingForwardHash = Animator.StringToHash("isWalkingForward");
        isWalkingBackwardsHash = Animator.StringToHash("isWalkingBackwards");
        isWalkingRightHash = Animator.StringToHash("isWalkingRight");
        isWalkingLeftHash = Animator.StringToHash("isWalkingLeft");
    }


    private void OnEnable()
    {
        playerControls.Player.Enable();

        jumpAction.performed += Jump;
        grappleAction.started += Grapple;
        pauseAction.performed += Pause;
    }


    private void OnDisable()
    {
        jumpAction.performed -= Jump;
        grappleAction.started -= Grapple;
        pauseAction.performed -= Pause;

        playerControls.Player.Disable();
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



        if (!isWalkingForward && pressedForward && playerGrounded)
        {
            playerAnimator.SetBool(isWalkingForwardHash, true);
        }
        if (isWalkingForward && !pressedForward || !playerGrounded)
        {
            playerAnimator.SetBool(isWalkingForwardHash, false);
        }


        if (!isWalkingBackwards && pressedBackwards && playerGrounded)
        {
            playerAnimator.SetBool(isWalkingBackwardsHash, true);
        }
        if (isWalkingBackwards && !pressedBackwards || !playerGrounded)
        {
            playerAnimator.SetBool(isWalkingBackwardsHash, false);
        }


        if (!isWalkingRight && pressedRight && playerGrounded)
        {
            playerAnimator.SetBool(isWalkingRightHash, true);
        }
        if (isWalkingRight && !pressedRight || !playerGrounded)
        {
            playerAnimator.SetBool(isWalkingRightHash, false);
        }

        if (!isWalkingLeft && pressedLeft && playerGrounded)
        {
            playerAnimator.SetBool(isWalkingLeftHash, true);
        }
        if (isWalkingLeft && !pressedLeft || !playerGrounded)
        {
            playerAnimator.SetBool(isWalkingLeftHash, false);
        }

        if (playerJumpedRecently && playerGrounded)
        {            
            playerAnimator.SetTrigger("landingTrigger");
            Debug.Log("Landing!");
            playerJumpedRecently = false;
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
        playerGrounded = CheckIfPlayerGrounded();
        MovePlayer();

        void MovePlayer()
        {
            playerRigidbody.drag = playerGrounded ? 10f : 0.1f;

            float airModifier = playerGrounded ? 1f : 0.5f;
            float maxSpeed = playerGrounded ? this.maxSpeed : maxSpeedAir;

            moveDirection *= playerSpeed * airModifier * Time.fixedDeltaTime;

            Vector2 rigidbodyVelocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);

            float force = playerGrounded ? forceModifier : forceModifier * 0.2f;

            if (rigidbodyVelocity.magnitude < maxSpeed)
            {
                playerRigidbody.AddForce(force * moveDirection);
            }
        }
    }


    private bool CheckIfPlayerGrounded() => Physics.CheckSphere(groundChecker.position, groundDistance, jumpLayer, QueryTriggerInteraction.Ignore);


    private void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPlayerPaused.TriggerEvent();
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!playerGrounded)
            return;

        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;


        playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        SoundManager.Instance.PlaySound(SoundManager.Sound.Jump);
        PlayJumpAnimation();
    }
    private void PlayJumpAnimation() => StartCoroutine(nameof(JumpedRecently));
    private void Grapple(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            hook.StartGrapple();
            
            if (!playerJumpedRecently)
            {
                PlayJumpAnimation();
            }
        }
    }

    IEnumerator JumpedRecently()
    {
        playerAnimator.SetTrigger("jumpTrigger");
        yield return new WaitForSeconds(0.3f);
        Debug.Log("jumpedRecently");
        playerJumpedRecently = true;
    }
}
