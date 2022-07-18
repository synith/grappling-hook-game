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
            // Load Current Scene
            Debug.Log("Loading Current Scene...");
        });
        mainMenuBtn.onClick.AddListener(() => 
        {
            // Load Main Menu Scene
            Debug.Log("Loading Main Menu Scene...");
        });
    }

    private void Start()
    {
        CollectableCounter.Instance.OnAllCollectablesCollected += AllCollectablesCollected;
        Hide();
    }
    private void AllCollectablesCollected(object sender, System.EventArgs e)
    {
        Show();        
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
