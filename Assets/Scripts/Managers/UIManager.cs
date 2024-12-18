using System;
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
        public GameObject gameOverPanel;

        [Header("Scores")]
        public Text scoreText;
        [Header("Images")]
        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;

        public int health;

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
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
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
        public void UpdateHealth()
        {
            foreach (Image img in hearts)
            {
                img.sprite = emptyHeart;
            }

            for (int i = 0; i < health && i < hearts.Length; i++)
            {
                hearts[i].sprite = fullHeart;
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

        public void ShowGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }

        public void HideGameOverPanel()
        {
            gameOverPanel.SetActive(false);
        }
    }
}