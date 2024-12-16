using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Panels")]
        public GameObject mainMenuPanel;
        public GameObject settingsPanel;
        public GameObject pausePanel;

        [Header("Texts")]
        public Text scoreText;
        public Text healthText;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Show a specific panel
        public void ShowPanel(GameObject panel)
        {
            HideAllPanels();
            panel.SetActive(true);
        }

        // Hide all panels
        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        // Update the score text
        public void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }

        // Update the health text
        public void UpdateHealth(int health)
        {
            if (healthText != null)
            {
                healthText.text = "Health: " + health;
            }
        }

        // Toggle pause panel visibility
        public void TogglePausePanel()
        {
            if (pausePanel != null)
            {
                bool isActive = pausePanel.activeSelf;
                pausePanel.SetActive(!isActive);
            }
        }
    }
}