using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertCameraY : MonoBehaviour
{

    private void OnEnable()
    {
        OptionsUI.onInvertVerticalCamera += InvertCameraAxisY;
    }

    private void OnDisable()
    {
        OptionsUI.onInvertVerticalCamera -= InvertCameraAxisY;
    }

    private void InvertCameraAxisY(bool invert)
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachinePOV virtualCameraAimSettings = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        virtualCameraAimSettings.m_VerticalAxis.m_InvertInput = !invert;
    }
}
