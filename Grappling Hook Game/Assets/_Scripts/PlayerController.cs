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
    private Vector3 _playerVelocity;
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

    private void Update()
    {
        _isPlayerGrounded = Physics.CheckSphere(_groundChecker.position, _groundDistance, _jumpLayer, QueryTriggerInteraction.Ignore);
        if (_isPlayerGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector2 input = _moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // moves player relative to camera direction
        move = move.x * _cameraTransform.right.normalized + move.z * _cameraTransform.forward.normalized;
        move.y = 0f;
        transform.position += move * _playerSpeed * Time.deltaTime;

        // jump
        if (_jumpAction.triggered && _isPlayerGrounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        // gravity
        _playerVelocity.y += _gravityValue * Time.deltaTime;

        // momentum
        transform.position += _playerVelocity * Time.deltaTime;

        // rotate toward camera direction
        float targetAngle = _cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
