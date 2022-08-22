using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField]
    private int _priorityBoostAmount = 10;
    [SerializeField]
    private Canvas _thirdPersonCanvas;
    [SerializeField]
    private Canvas _aimCanvas;

    private CinemachineVirtualCamera _virtualCamera;
    private PlayerControls playerControls;

    private InputAction _aimAction;



    private void Awake()
    {
        playerControls = new PlayerControls();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _aimAction = playerControls.Player.Aim;
        _aimCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        _aimAction.performed += StartAim;
        _aimAction.canceled += CancelAim;
    }
    private void OnDisable()
    {
        _aimAction.performed -= StartAim;
        _aimAction.canceled -= CancelAim;
        playerControls.Player.Disable();
    }

    private void CancelAim(InputAction.CallbackContext context)
    {
        _virtualCamera.Priority -= _priorityBoostAmount;
        _aimCanvas.gameObject.SetActive(false);
        _thirdPersonCanvas.gameObject.SetActive(true);
    }

    private void StartAim(InputAction.CallbackContext context)
    {
        _virtualCamera.Priority += _priorityBoostAmount;
        _thirdPersonCanvas.gameObject.SetActive(false);
        _aimCanvas.gameObject.SetActive(true);
        
    }

    
}
