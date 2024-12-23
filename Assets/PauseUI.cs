using Managers;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void OnResumeGame()
    {
        UIManager.Instance.OnResumeGame();
    }

    public void OnOptionsGame()
    {
        
    }

    public void OnMainMenu()
    {
        SceneManagers.Instance.LoadScene("StartScene");
        UIManager.Instance.HideAllPanels();
    }

    public void OnPauseGame()
    {
        UIManager.Instance.OnPauseGame();
    }
}
