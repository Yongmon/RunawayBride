using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnNewGame()
    {
        Debug.Log("新游戏");
        SceneManager.LoadScene("GameScene");
    }

    public void OnContinueGame()
    {
        if (PlayerPrefs.HasKey("SaveExists"))
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("没有存档");
        }
    }

    public void OnQuitGame()
    {
        Debug.Log("退出");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}