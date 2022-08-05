using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class OptionsUI : MonoBehaviour
{
    public static event Action<float> onSetSensitivity;

    private TextMeshProUGUI
        sensitivityValueText,
        musicVolumeText,
        soundVolumeText;

    private Slider sensitivitySlider;

    private const float SENSITIVITY_COEFFICIENT = 2f;
    private const float DEFAULT_SENSITIVITY = 50f;
    private const string MOUSE_SENSITIVITY = "mouseSensitivity";


    void Awake()
    {
        musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();
        soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        sensitivityValueText = transform.Find("sensitivityValueText").GetComponent<TextMeshProUGUI>();
        sensitivitySlider = transform.Find("sensitivitySlider").GetComponent<Slider>();
    }


    void Start()
    {
        SliderInit(sensitivitySlider);
        ButtonInit();
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
        UpdateOptionsText();
        GameManager.Instance.UpdateGameState(GameManager.GameState.Paused);
        gameObject.SetActive(true);
        
    }

    void Unpause()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
        gameObject.SetActive(false);
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
}
