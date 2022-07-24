using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV virtualCameraAimSettings;

    private float defaultHorizontalSensitivity;
    private float defaultVerticalSensitivity;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraAimSettings = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        defaultHorizontalSensitivity = virtualCameraAimSettings.m_HorizontalAxis.m_MaxSpeed;
        defaultVerticalSensitivity = virtualCameraAimSettings.m_VerticalAxis.m_MaxSpeed;
    }

    public void SetSensitivity(float value)
    {        
        float horizontalSensitivity = defaultHorizontalSensitivity * value;
        float verticalSensitivity = defaultVerticalSensitivity * value;

        virtualCameraAimSettings.m_HorizontalAxis.m_MaxSpeed = horizontalSensitivity;
        virtualCameraAimSettings.m_VerticalAxis.m_MaxSpeed = verticalSensitivity;
    }
}
