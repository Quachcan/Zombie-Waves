using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Transform PlayerTransform { get; private set; }
        [SerializeField]
        private bool isGameOver;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            isGameOver = false;
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }


        private void GameOver()
        {
            if(isGameOver)
                return;
            isGameOver = true;
            UIManager.Instance.ShowGameOverPanel();
        }

        public void RestartGame()
        {
            UIManager.Instance.HideGameOverPanel();
            SceneHandler.Instance.ReloadScene();
        }

        public void LoadScene(string sceneName)
        {
            //  SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OnPlayerDeath()
        {
            GameOver();
        }

        public void OnPlayerTakeDamage(int currentHealth)
        {
            UIManager.Instance.health = currentHealth;
            UIManager.Instance.UpdateHealth();
        }
    }
}
