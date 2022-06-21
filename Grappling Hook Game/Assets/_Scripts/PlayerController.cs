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
    private float _maxSpeed;

    private Transform _cameraTransform;
    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;
    [SerializeField]
    private bool _playerGrounded;

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
    private void OnJump()
    {
        if (_playerGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            _playerGrounded = false;
        }
    }
    private void Update()
    {
        _moveDirection = PlayerInputVector();
        _moveDirection = ToCameraDirectionMovement(_moveDirection, _cameraTransform);

        // rotate player toward camera direction
        float targetAngle = _cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        Vector3 PlayerInputVector()
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            return move.normalized;
        }

        Vector3 ToCameraDirectionMovement(Vector3 movementDirection, Transform cameraTransform)
        {
            movementDirection = movementDirection.x * cameraTransform.right.normalized + movementDirection.z * cameraTransform.forward.normalized;
            movementDirection.y = 0f;
            return movementDirection;
        }
    }
    private void FixedUpdate()
    {
        _playerGrounded = Physics.CheckSphere(_groundChecker.position, _groundDistance, _jumpLayer, QueryTriggerInteraction.Ignore);
        float airSpeedModifier = _playerGrounded ? 1f : 0.7f;

        //move

        if (_rigidbody.velocity.magnitude < _maxSpeed)
        {
            _rigidbody.AddForce(_moveDirection * _playerSpeed * airSpeedModifier, ForceMode.Force);
        }

    }
} 
