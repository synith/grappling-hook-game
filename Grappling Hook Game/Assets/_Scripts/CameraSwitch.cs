using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private int _priorityBoostAmount = 10;
    [SerializeField]
    private Canvas _thirdPersonCanvas;
    [SerializeField]
    private Canvas _aimCanvas;

    private CinemachineVirtualCamera _virtualCamera;

    private InputAction _aimAction;
    

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _aimAction = _playerInput.actions["Aim"];
        _aimCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _aimAction.performed += _ => StartAim();
        _aimAction.canceled += _ => CancelAim();
    }
    private void OnDisable()
    {
        _aimAction.performed -= _ => StartAim();
        _aimAction.canceled -= _ => CancelAim();
    }

    private void CancelAim()
    {
        _virtualCamera.Priority -= _priorityBoostAmount;
        _aimCanvas.gameObject.SetActive(false);
        _thirdPersonCanvas.gameObject.SetActive(true);
    }

    private void StartAim()
    {
        _virtualCamera.Priority += _priorityBoostAmount;
        _thirdPersonCanvas.gameObject.SetActive(false);
        _aimCanvas.gameObject.SetActive(true);
        
    }

    
}
