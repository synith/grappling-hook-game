using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class OptionsUI : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    public static Action<float> onSetSensitivity;

    private bool isPaused;

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
        Hide(playSound: false);

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
            SetupButton("musicIncreaseBtn", () => musicManager.IncreaseVolume());
            SetupButton("musicDecreaseBtn", () => musicManager.DecreaseVolume());
            SetupButton("soundIncreaseBtn", () => SoundManager.Instance.IncreaseVolume());
            SetupButton("soundDecreaseBtn", () => SoundManager.Instance.DecreaseVolume());

            SetupSceneTransferButton("mainMenuBtn", GameSceneManager.Scene.Main_Menu_Scene);

            void SetupButton(string buttonName, Action buttonAction)
            {
                transform.Find(buttonName).GetComponent<Button>().onClick.AddListener(() =>
                {
                    buttonAction();
                    UpdateOptionsText();
                    ButtonSound();
                });
            }

            void SetupSceneTransferButton(string buttonName, GameSceneManager.Scene scene)
            {
                transform.Find(buttonName).GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameSceneManager.Load(scene);
                    ButtonSound();
                });
            }

            void ButtonSound() => SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
        }
    }


    void UpdateOptionsText()
    {
        sensitivityValueText.SetText(sensitivitySlider.value.ToString());
        soundVolumeText.SetText(Mathf.RoundToInt(SoundManager.Instance.Volume * 10).ToString());
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.Volume * 10).ToString());
    }


    void Show()
    {
        UpdateOptionsText();
        isPaused = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        SoundManager.Instance.PlaySound(SoundManager.Sound.Pause);
    }


    void Hide(bool playSound = true)
    {
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        if (playSound)
            SoundManager.Instance.PlaySound(SoundManager.Sound.Unpause);
    }


    public void Toggle()
    {
        if (isPaused)
            Hide();
        else
            Show();
    }
}
