using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;

    [SerializeField] private MusicManager musicManager;

    private bool isPaused;

    private TextMeshProUGUI
        sensitivityValueText,
        musicVolumeText,
        soundVolumeText;

    private Slider sensitivitySlider;

    private void Awake()
    {
        SliderInit();

        ButtonInit();

        Hide();

        void SliderInit()
        {
            sensitivityValueText = transform.Find("sensitivityValueText").GetComponent<TextMeshProUGUI>();
            sensitivitySlider = transform.Find("sensitivitySlider").GetComponent<Slider>();

            sensitivitySlider.onValueChanged.AddListener(sliderValue =>
            {
                float sensitivityModifierValue = 2f * sliderValue / sensitivitySlider.maxValue;

                thirdPersonCamera?.GetComponent<CameraSensitivity>().SetSensitivity(sensitivityModifierValue);
                aimCamera?.GetComponent<CameraSensitivity>().SetSensitivity(sensitivityModifierValue);
                UpdateText();
            });
        }

        void ButtonInit()
        {
            musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();
            soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();

            transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameSceneManager.Load(GameSceneManager.Scene.Main_Menu_Scene);
                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            });

            transform.Find("musicIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                musicManager.IncreaseVolume();
                UpdateText();
                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            });

            transform.Find("musicDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                musicManager.DecreaseVolume();
                UpdateText();
                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            });

            transform.Find("soundIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.Instance.IncreaseVolume();
                UpdateText();
                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            });

            transform.Find("soundDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.Instance.DecreaseVolume();
                UpdateText();
                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            });
        }        
    }

    private void UpdateText()
    {
        sensitivityValueText.SetText(sensitivitySlider.value.ToString());
        soundVolumeText.SetText(Mathf.RoundToInt(SoundManager.Instance.Volume * 10).ToString());
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.Volume * 10).ToString());
    }

    private void Show()
    {
        isPaused = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void Hide()
    {
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void Toggle()
    {
        if (isPaused)
            Hide();
        else
            Show();
    }
}


