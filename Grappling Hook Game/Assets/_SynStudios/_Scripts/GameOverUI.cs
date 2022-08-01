using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private Button retryBtn;
    private Button mainMenuBtn;


    private void Awake()
    {
        retryBtn = transform.Find("retryBtn").GetComponent<Button>();
        mainMenuBtn = transform.Find("mainMenuBtn").GetComponent<Button>();

        retryBtn.onClick.AddListener(() => 
        {
            GameSceneManager.Load(GameSceneManager.Scene.Main_Game_Scene);
        });
        mainMenuBtn.onClick.AddListener(() => 
        {
            GameSceneManager.Load(GameSceneManager.Scene.Main_Menu_Scene);
        });
    }
    private void OnEnable()
    {
        CollectableCounter.OnGameOver += () => AllCollectablesCollected();
    }

    private void OnDisable()
    {
        CollectableCounter.OnGameOver -= () => AllCollectablesCollected();
    }
    private void Start()
    {         
        Hide();
    }


    private void AllCollectablesCollected()
    {
        Show();
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameWon);
    }


    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }


    public void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
