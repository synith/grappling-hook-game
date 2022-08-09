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
            GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);          
        });
        mainMenuBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Main_Menu);
        });
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void FinishGame()
    {
        gameObject.SetActive(true);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Finished);
    }
}
