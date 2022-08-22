using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInput : MonoBehaviour
{
    public static Vector2 InputDirection { get; private set; }
    public static bool IsRunning { get; private set; }

    PlayerControls playerControls;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction grappleAction;
    InputAction pauseAction;
    InputAction runAction;


    [SerializeField] GameEventSO onPlayerPaused;

    public static event Action
        OnJump,
        OnStartGrapple,
        OnStopGrapple,
        OnPause;


    void Awake()
    {
        playerControls = new PlayerControls();

        moveAction = playerControls.Player.Move;
        jumpAction = playerControls.Player.Jump;
        grappleAction = playerControls.Player.Grapple;
        pauseAction = playerControls.Player.Pause;
        runAction = playerControls.Player.Run;
    }
    void OnEnable()
    {
        playerControls.Player.Enable();

        jumpAction.performed += Jump;
        grappleAction.started += StartGrapple;
        grappleAction.canceled += StopGrapple;
        pauseAction.performed += Pause;
        runAction.performed += StartRunning;
        runAction.canceled += StopRunning;
    }
    void OnDisable()
    {
        jumpAction.performed -= Jump;
        grappleAction.started -= StartGrapple;
        grappleAction.canceled -= StopGrapple;
        pauseAction.performed -= Pause;
        runAction.performed -= StartRunning;
        runAction.canceled -= StopRunning;

        playerControls.Player.Disable();
    }

    private void StopRunning(InputAction.CallbackContext context)
    {
        if (!context.canceled) return;
        IsRunning = false;
    }

    private void StartRunning(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        IsRunning = true;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        onPlayerPaused.TriggerEvent();
    }

    private void StartGrapple(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        OnStartGrapple.Invoke();
    }

    private void StopGrapple(InputAction.CallbackContext context)
    {
        if (!context.canceled) return;
        OnStopGrapple.Invoke();
    }    

    void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnJump.Invoke();
    }

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        InputDirection = moveAction.ReadValue<Vector2>();
    }
}
