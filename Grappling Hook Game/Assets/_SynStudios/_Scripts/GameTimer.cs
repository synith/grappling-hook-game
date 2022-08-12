using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI gameOverTimeTakenText;

    StopWatch stopWatch;

    void Start()
    {
        stopWatch = new();
        stopWatch.Start();
    }
    void Update()
    {
        stopWatch.Update();
    }

    private void SetGameTimerText()
    {
        gameOverTimeTakenText.SetText($"It took you {stopWatch.Seconds:00}s to complete the game");
    }

    private void OnEnable()
    {
        GameOverUI.onGameFinished += SetGameTimerText;
    }   

    private void OnDisable()
    {
        GameOverUI.onGameFinished -= SetGameTimerText;
    }

    
}
