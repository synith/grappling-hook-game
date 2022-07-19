using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _thirdPersonCamera;
    [SerializeField] CinemachineVirtualCamera _aimCamera;

    private bool _isPaused;

    private void Awake()
    {
        TextMeshProUGUI sensitivityValueText = transform.Find("sensitivityValueText").GetComponent<TextMeshProUGUI>();
        Slider sensitivitySlider = transform.Find("sensitivitySlider").GetComponent<Slider>();

        sensitivitySlider.onValueChanged.AddListener(sliderValue =>
        {
            float sensitivityModifierValue = 2f * sliderValue / sensitivitySlider.maxValue;

            _thirdPersonCamera.GetComponent<CameraSensitivity>().SetSensitivity(sensitivityModifierValue);
            _aimCamera.GetComponent<CameraSensitivity>().SetSensitivity(sensitivityModifierValue);

            sensitivityValueText.SetText(sliderValue.ToString());
        });

        Hide();
    }



    private void Show()
    {
        _isPaused = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void Hide()
    {
        _isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Toggle()
    {
        if (_isPaused)
            Hide();
        else
            Show();
    }
}


