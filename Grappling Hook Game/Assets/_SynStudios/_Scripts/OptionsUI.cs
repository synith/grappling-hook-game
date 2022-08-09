using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class OptionsUI : MonoBehaviour
{
    public static event Action<float> onSetSensitivity;
    public static event Action<bool> onInvertVerticalCamera;

    private TextMeshProUGUI
        sensitivityValueText,
        musicVolumeText,
        soundVolumeText;

    private Toggle invertCameraToggle;

    private Slider sensitivitySlider;

    private bool cameraInverted;

    private const float SENSITIVITY_COEFFICIENT = 2f;
    private const float DEFAULT_SENSITIVITY = 50f;
    private const string MOUSE_SENSITIVITY = "mouseSensitivity";
    private const string CAMERA_INVERTED = "cameraInverted";


    void Awake()
    {
        musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();
        soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        sensitivityValueText = transform.Find("sensitivityValueText").GetComponent<TextMeshProUGUI>();
        sensitivitySlider = transform.Find("sensitivitySlider").GetComponent<Slider>();
        invertCameraToggle = transform.Find("invertCameraToggle").GetComponent<Toggle>();

        cameraInverted = intToBool(PlayerPrefs.GetInt(CAMERA_INVERTED));
        onInvertVerticalCamera?.Invoke(cameraInverted);
    }

    void Start()
    {
        SliderInit(sensitivitySlider);
        ButtonInit();

        invertCameraToggle.onValueChanged.AddListener((bool set) =>
        {
            PlayerPrefs.SetInt(CAMERA_INVERTED, boolToInt(invertCameraToggle.isOn));
            onInvertVerticalCamera?.Invoke(set);
        });

        onInvertVerticalCamera?.Invoke(cameraInverted);
        invertCameraToggle.isOn = cameraInverted;


        gameObject.SetActive(false);

        void SliderInit(Slider sensitivitySlider)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY, DEFAULT_SENSITIVITY);

            float sliderValueNormalized = sensitivitySlider.value / sensitivitySlider.maxValue;
            float sensitivityValue = SENSITIVITY_COEFFICIENT * sliderValueNormalized;
            onSetSensitivity?.Invoke(sensitivityValue);

            SetupSlider(sensitivitySlider);

            void SetupSlider(Slider sensitivitySlider)
            {
                sensitivitySlider.onValueChanged.AddListener(sliderValue =>
                {
                    PlayerPrefs.SetFloat(MOUSE_SENSITIVITY, sliderValue);

                    float sliderValueNormalized = sliderValue / sensitivitySlider.maxValue;
                    float sensitivityValue = SENSITIVITY_COEFFICIENT * sliderValueNormalized;

                    UpdateOptionsText();

                    sensitivityValue = SENSITIVITY_COEFFICIENT * sliderValue / sensitivitySlider.maxValue;

                    onSetSensitivity?.Invoke(sensitivityValue);
                });
            }
        }

        void ButtonInit()
        {

            SetupButton("musicIncreaseBtn", () => MusicManager.Instance.IncreaseVolume());
            SetupButton("musicDecreaseBtn", () => MusicManager.Instance.DecreaseVolume());
            SetupButton("soundIncreaseBtn", () => SoundManager.Instance.IncreaseVolume());
            SetupButton("soundDecreaseBtn", () => SoundManager.Instance.DecreaseVolume());

            SetupButton("mainMenuBtn", () => GameManager.Instance.UpdateGameState(GameManager.GameState.Main_Menu));

            void SetupButton(string buttonName, Action buttonAction)
            {
                transform.Find(buttonName).GetComponent<Button>().onClick.AddListener(() =>
                {
                    buttonAction();
                    UpdateOptionsText();
                    SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
                });
            }
        }
    }


    void UpdateOptionsText()
    {
        sensitivityValueText.SetText(sensitivitySlider.value.ToString());
        soundVolumeText.SetText(Mathf.RoundToInt(SoundManager.Instance.Volume * 10).ToString());
        musicVolumeText.SetText(Mathf.RoundToInt(MusicManager.Instance.Volume * 10).ToString());
    }


    void Pause()
    {

        gameObject.SetActive(true);
        UpdateOptionsText();
        GameManager.Instance.UpdateGameState(GameManager.GameState.Paused);
    }

    void Unpause()
    {
        gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
    }


    public void Toggle()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.Paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    int boolToInt(bool value)
    {
        if (value)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    bool intToBool(int value)
    {
        if (value != 0)
        {
            return true;
        }
        else return false;
    }
}
