using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Collections.Generic;
using System;

//public class MusicVolumeTextMeshPro : MonoBehaviour
//{

//}

public class OptionsUI : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;

    [SerializeField] MusicManager musicManager;



    private bool isPaused;

    private TextMeshProUGUI
        sensitivityValueText,
        musicVolumeText,
        soundVolumeText;

    private Slider sensitivitySlider;

    private const float SENSITIVITY_FACTOR = 2f;
    private const float DEFAULT_SENSITIVITY = 50f;
    private const string MOUSE_SENSITIVITY = "mouseSensitivity";


    void Awake()
    {
        //musicVolumeText = FindObjectOfType<MusicVolumeTextMeshPro>().GetComponent<TextMeshProUGUI>(); // good tip, refactor like this to avoid breaking things with string
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
            float sensitivityModifierValue = SENSITIVITY_FACTOR * sensitivitySlider.value / sensitivitySlider.maxValue;
            SetCameraSensitivity(thirdPersonCamera, SENSITIVITY_FACTOR * sensitivitySlider.value / sensitivitySlider.maxValue);
            SetCameraSensitivity(aimCamera, SENSITIVITY_FACTOR * sensitivitySlider.value / sensitivitySlider.maxValue);

            sensitivitySlider.onValueChanged.AddListener(sliderValue =>
            {
                PlayerPrefs.SetFloat(MOUSE_SENSITIVITY, sliderValue);
                UpdateText();

                sensitivityModifierValue = SENSITIVITY_FACTOR * sliderValue / sensitivitySlider.maxValue;
                SetCameraSensitivity(thirdPersonCamera, sensitivityModifierValue);
                SetCameraSensitivity(aimCamera, sensitivityModifierValue);
            });


            void SetCameraSensitivity(CinemachineVirtualCamera virtualCamera, float sensitivityModifierValue)
                => virtualCamera.GetComponent<CameraSensitivity>().SetSensitivity(sensitivityModifierValue);
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
                    UpdateText();
                    PlayButtonPressedSound();
                });
            }


            void SetupSceneTransferButton(string buttonName, GameSceneManager.Scene scene)
            {
                transform.Find(buttonName).GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameSceneManager.Load(scene);
                    PlayButtonPressedSound();
                });
            }
        }
    }


    void PlayButtonPressedSound()
        => SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);


    void UpdateText()
    {
        sensitivityValueText.SetText(sensitivitySlider.value.ToString());
        soundVolumeText.SetText(Mathf.RoundToInt(SoundManager.Instance.Volume * 10).ToString());
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.Volume * 10).ToString());
    }


    void Show()
    {
        UpdateText();
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
