using System;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public static event Action OnLoadScene;
    public enum Scene
    {
        Main_Game_Scene,
        Main_Menu_Scene,
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
        OnLoadScene?.Invoke();
    }
}
