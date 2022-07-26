using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Collections.Generic;
using System;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;

    [SerializeField] MusicManager musicManager;

    bool isPaused;

    TextMeshProUGUI
        sensitivityValueText,
        musicVolumeText,
        soundVolumeText;

    Slider sensitivitySlider;


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
        Hide();


        void SliderInit(Slider sensitivitySlider)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivity", 50f);
            SetCameraSensitivity(thirdPersonCamera, 2f * sensitivitySlider.value / sensitivitySlider.maxValue);
            SetCameraSensitivity(aimCamera, 2f * sensitivitySlider.value / sensitivitySlider.maxValue);

            sensitivitySlider.onValueChanged.AddListener(sliderValue =>
            {
                PlayerPrefs.SetFloat("mouseSensitivity", sliderValue);
                UpdateText();

                float sensitivityModifierValue = 2f * sliderValue / sensitivitySlider.maxValue;
                SetCameraSensitivity(thirdPersonCamera, sensitivityModifierValue);
                SetCameraSensitivity(aimCamera, sensitivityModifierValue);
            });


            void SetCameraSensitivity(CinemachineVirtualCamera virtualCamera, float sensitivityModifierValue)
                => virtualCamera.GetComponent<CameraSensitivity>().SetSensitivity(sensitivityModifierValue);
        }


        void ButtonInit()
        {
            Action increaseMusicVolume = () => musicManager.IncreaseVolume();
            Action decreaseMusicVolume = () => musicManager.DecreaseVolume();
            Action increaseSoundVolume = () => SoundManager.Instance.IncreaseVolume();
            Action decreaseSoundVolume = () => SoundManager.Instance.DecreaseVolume();

            Dictionary<string, Action> soundButtonActionDictionary = new Dictionary<string, Action>();

            soundButtonActionDictionary.Add("musicIncreaseBtn", increaseMusicVolume);
            soundButtonActionDictionary.Add("musicDecreaseBtn", decreaseMusicVolume);
            soundButtonActionDictionary.Add("soundIncreaseBtn", increaseSoundVolume);
            soundButtonActionDictionary.Add("soundDecreaseBtn", decreaseSoundVolume);

            foreach (string soundButton in soundButtonActionDictionary.Keys)
            {
                Action soundButtonAction = soundButtonActionDictionary[soundButton];
                SetupButton(soundButton, soundButtonAction);
            }

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


    void Hide()
    {
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
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
