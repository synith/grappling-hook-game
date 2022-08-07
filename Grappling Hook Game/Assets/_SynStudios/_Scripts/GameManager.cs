using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Main_Menu,
        Playing,
        Paused,
        Finished,
    }

    public GameState currentState { get; private set; } = GameState.Main_Menu;
    private GameState lastState;

    public static GameManager Instance { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void UpdateGameState(GameState newState)
    {
        lastState = currentState;
        currentState = newState;
        print($"State Changed to: {newState}");
        switch (newState)
        {
            case GameState.Main_Menu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.Finished:
                HandleFinished();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu()
    {
        if (lastState == GameState.Paused || lastState == GameState.Finished)
        {
            MusicManager.Instance.PlayMusic(MusicManager.Music.Menu);
            GameSceneManager.Load(GameSceneManager.Scene.Menu_Scene);
        }
    }

    private void HandleFinished()
    {
        if (lastState == GameState.Playing)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            SoundManager.Instance.PlaySound(SoundManager.Sound.GameWon);
        }
    }

    private void HandlePlaying()
    {
        if (lastState == GameState.Main_Menu || lastState == GameState.Paused || lastState == GameState.Finished)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            SoundManager.Instance.PlaySound(SoundManager.Sound.Unpause);

            if (lastState != GameState.Paused)
            {
                MusicManager.Instance.PlayMusic(MusicManager.Music.Game);
                GameSceneManager.Load(GameSceneManager.Scene.Game_Scene);                
            }
        }
    }

    private void HandlePaused()
    {
        if (lastState == GameState.Playing)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            SoundManager.Instance.PlaySound(SoundManager.Sound.Pause);
        }
    }
}