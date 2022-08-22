using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float
        playerSpeedSetting,
        jumpHeight,
        rotationSpeed,
        forceModifier,
        maxSpeedSetting,
        maxSpeedAir,
        groundDistance;

    float speed;
    float maxSpeed;

    [SerializeField]
    LayerMask jumpLayer;

    public static bool Grounded { get; private set; }

    Vector3 moveDirection;

    GrapplingHook hook;

    Rigidbody playerRigidbody;
    Transform cameraTransform;
    Transform groundChecker;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
        hook = GetComponent<GrapplingHook>();
        playerRigidbody = GetComponent<Rigidbody>();
        groundChecker = transform.Find("groundChecker");
    }

    void OnEnable()
    {
        PlayerInput.OnJump += Jump;
        PlayerInput.OnStartGrapple += StartGrapple;
        PlayerInput.OnStopGrapple += StopGrapple;

    }

    void OnDisable()
    {
        PlayerInput.OnJump -= Jump;
        PlayerInput.OnStartGrapple -= StartGrapple;
        PlayerInput.OnStopGrapple -= StopGrapple;

    }

    void Update()
    {
        RotatePlayerTowardsCamera();
        SetMovementDirectionFromInputAndCamera();
        CheckIfRunning();

        void CheckIfRunning()
        {
            if (PlayerInput.IsRunning)
            {
                maxSpeed = maxSpeedSetting * 2f;
                speed = playerSpeedSetting * 1.5f;
            }
            else
            {
                maxSpeed = maxSpeedSetting;
                speed = playerSpeedSetting;
            }
        }

        void SetMovementDirectionFromInputAndCamera()
        {
            moveDirection = PlayerInputVectorNormalized();
            moveDirection = MovementDirectionRelativeToCamera(moveDirection, cameraTransform);
            moveDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up).normalized;

            Vector3 PlayerInputVectorNormalized()
            {
                Vector2 input = PlayerInput.InputDirection;
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


    void FixedUpdate()
    {
        Grounded = CheckIfPlayerGrounded();
        MovePlayer();

        void MovePlayer()
        {
            playerRigidbody.drag = Grounded ? 10f : 0.1f;

            float airModifier = Grounded ? 1f : 0.5f;
            float maxSpeed = Grounded ? this.maxSpeed : maxSpeedAir;

            moveDirection *= speed * airModifier * Time.fixedDeltaTime;

            Vector2 rigidbodyVelocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);

            float force = Grounded ? forceModifier : forceModifier * 0.2f;

            if (rigidbodyVelocity.magnitude < maxSpeed)
            {
                playerRigidbody.AddForce(force * moveDirection);
            }
        }
    }


    bool CheckIfPlayerGrounded() => Physics.CheckSphere(groundChecker.position, groundDistance, jumpLayer, QueryTriggerInteraction.Ignore);

    void Jump()
    {
        if (!Grounded)
            return;

        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;


        playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        SoundManager.Instance.PlaySound(SoundManager.Sound.Jump);
    }
    void StartGrapple() => hook.StartGrapple();

    void StopGrapple() => hook.StopGrapple();

}
