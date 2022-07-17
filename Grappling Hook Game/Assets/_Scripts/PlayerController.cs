using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private float _jumpHeight;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _forceModifier;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxSpeedAir;
    [SerializeField]
    private float _groundDistance;
    [SerializeField]
    private LayerMask _jumpLayer;

    private bool _isPlayerGrounded;
    private Vector3 _moveDirection;

    private GrapplingHook _hook;

    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private Transform _groundChecker;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _grappleAction;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;

        _hook = GetComponent<GrapplingHook>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();

        _groundChecker = transform.Find("groundChecker");

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _grappleAction = _playerInput.actions["Grapple"];

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        _jumpAction.performed += _ => OnJump();
        _grappleAction.started += _ => _hook.StartGrapple();
    }
    private void OnDisable()
    {
        _jumpAction.performed -= _ => OnJump();
        _grappleAction.started -= _ => _hook.StartGrapple();
    }
    private void Update()
    {
        CheckIfGrapplingStopped();
        RotatePlayerTowardsCamera();
        SetMovementDirectionFromInputAndCamera();


        void CheckIfGrapplingStopped()
        {
            if (_grappleAction.WasReleasedThisFrame())
            {
                _hook.StopGrapple();
            }
        }
        void SetMovementDirectionFromInputAndCamera()
        {
            _moveDirection = PlayerInputVectorNormalized();
            _moveDirection = MovementDirectionRelativeToCamera(_moveDirection, _cameraTransform);
            _moveDirection = Vector3.ProjectOnPlane(_moveDirection, Vector3.up).normalized;

            Vector3 PlayerInputVectorNormalized()
            {
                Vector2 input = _moveAction.ReadValue<Vector2>();
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
            float targetAngle = _cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        _isPlayerGrounded = Physics.CheckSphere(_groundChecker.position, _groundDistance, _jumpLayer, QueryTriggerInteraction.Ignore);
        MovePlayer();

        void MovePlayer()
        {
            _rigidbody.drag = _isPlayerGrounded ? 10f : 0.1f;

            float airModifier = _isPlayerGrounded ? 1f : 0.5f;
            float maxSpeed = _isPlayerGrounded ? _maxSpeed : _maxSpeedAir;

            _moveDirection *= _playerSpeed * airModifier * Time.fixedDeltaTime;

            Vector2 rigidbodyVelocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);

            float force = _isPlayerGrounded ? _forceModifier : _forceModifier * 0.2f;

            if (rigidbodyVelocity.magnitude < maxSpeed)
            {
                _rigidbody.AddForce(force * _moveDirection);
            }
        }
    }
    private void OnJump()
    {
        if (_isPlayerGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
        }
    }
}
