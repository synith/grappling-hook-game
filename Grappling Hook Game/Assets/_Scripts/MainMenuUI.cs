using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonPress);
            GameSceneManager.Load(GameSceneManager.Scene.Main_Game_Scene);
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
