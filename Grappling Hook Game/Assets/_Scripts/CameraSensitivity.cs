using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachinePOV _virtualCameraAimSettings;

    private float _defaultHorizontalSensitivity;
    private float _defaultVerticalSensitivity;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _virtualCameraAimSettings = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        _defaultHorizontalSensitivity = _virtualCameraAimSettings.m_HorizontalAxis.m_MaxSpeed;
        _defaultVerticalSensitivity = _virtualCameraAimSettings.m_VerticalAxis.m_MaxSpeed;
    }

    public void SetSensitivity(float value)
    {        
        float horizontalSensitivity = _defaultHorizontalSensitivity * value;
        float verticalSensitivity = _defaultVerticalSensitivity * value;

        _virtualCameraAimSettings.m_HorizontalAxis.m_MaxSpeed = horizontalSensitivity;
        _virtualCameraAimSettings.m_VerticalAxis.m_MaxSpeed = verticalSensitivity;
    }
}
