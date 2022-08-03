using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);            
        });
        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
