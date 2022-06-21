using System.Collections;
using System.Collections.Generic;
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
    private float _gravityValue;

    private Transform _cameraTransform;
    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;
    [SerializeField]
    private bool _isPlayerGrounded;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _grappleAction;

    [SerializeField]
    private LayerMask _jumpLayer;
    private Transform _groundChecker;
    [SerializeField]
    private float _groundDistance;


    private Vector3 _moveDirection;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
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
    }
    private void OnDisable()
    {
        _jumpAction.performed += _ => OnJump();
    }

    private void OnJump()
    {
        if (_isPlayerGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        _moveDirection = PlayerInputVector();
        _moveDirection = MovementRelativeToCamera(_moveDirection, _cameraTransform);

        RotatePlayerTowardsCamera();

        Vector3 PlayerInputVector()
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            Vector3 move = new(input.x, 0, input.y);
            return move.normalized;
        }

        Vector3 MovementRelativeToCamera(Vector3 movementDirection, Transform cameraTransform)
        {
            movementDirection = movementDirection.x * cameraTransform.right.normalized + movementDirection.z * cameraTransform.forward.normalized;
            movementDirection.y = 0f;
            return movementDirection;
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
            float airModifier = _isPlayerGrounded ? 1f : 0.8f;
            _moveDirection *= _playerSpeed * airModifier * Time.deltaTime;
            _rigidbody.velocity = new Vector3(_moveDirection.x, _rigidbody.velocity.y, _moveDirection.z);
        }
    }
}
