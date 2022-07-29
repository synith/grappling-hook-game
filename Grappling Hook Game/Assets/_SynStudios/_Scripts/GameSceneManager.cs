using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public enum Scene
    {
        Main_Game_Scene,
        Main_Menu_Scene,
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
