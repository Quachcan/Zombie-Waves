using Managers;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene_01");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
