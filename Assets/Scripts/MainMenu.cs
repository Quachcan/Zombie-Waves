using Managers;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManagers.Instance.LoadScene("GameScene_01");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
