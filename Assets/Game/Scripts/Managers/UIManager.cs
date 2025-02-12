using System.Collections.Generic;
using Game.Scripts.PlayerScripts;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Panels")]
        public GameObject mainMenuPanel;
        public GameObject joyStickSettingsPanel;
        public GameObject pausePanel;
        public GameObject gameOverPanel;
        public GameObject gameWinPanel;
        public GameObject expCanvas;
        public GameObject timerCanvas;

        [Header("Timer")]
        public TextMeshProUGUI timerText;
        [Header("Images")]
        public Sprite fullHeart;
        public Sprite emptyHeart;
        public GameObject heartPrefab;
        public Transform heartsParent;
        
        public List<Image> hearts;
        
        [SerializeField]
        private ExpUiManager expUiManager;

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
            
            expUiManager = GetComponentInChildren<ExpUiManager>();
        }

        public void Initialize()
        {
            SetupHearts(Player.Instance.playerHealth.MaxHealth);
            
            health = Player.Instance.playerHealth.CurrentHealth;
            UpdateHealthDisplay(health);
            
            Player.Instance.playerHealth.OnMaxHealthChanged += OnMaxHealthChanged;
            Player.Instance.playerHealth.OnCurrentHealthChanged += OnCurrentHealthChanged;
        }
        
        public void ShowPanel()
        {
            if (joyStickSettingsPanel != null) joyStickSettingsPanel.SetActive(true);
            if (expCanvas != null) expCanvas.SetActive(true);
            if (timerCanvas != null) timerCanvas.SetActive(true);
        }
        
        public void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (joyStickSettingsPanel != null) joyStickSettingsPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (expCanvas != null) expCanvas.SetActive(false);
            if (timerCanvas != null) timerCanvas.SetActive(false);
        }

        private void SetupHearts(int maxHealth)
        {
            foreach (Transform child in heartsParent)
            {
                Destroy(child.gameObject);
            }
            
            if(hearts == null)
                hearts = new List<Image>();
            else 
                hearts.Clear();
            
            for (int i = 0; i < maxHealth; i++)
            {
                GameObject heartObj = Instantiate(heartPrefab, heartsParent);
                heartObj.transform.SetAsFirstSibling();
                hearts.Add(heartObj.GetComponent<Image>());
            }
            
            health = Player.Instance.playerHealth.MaxHealth;
        }

        private void OnMaxHealthChanged(int maxHealth)
        {
            SetupHearts(maxHealth);
            UpdateHealthDisplay(Player.Instance.playerHealth.CurrentHealth);
        }

        private void OnCurrentHealthChanged(int currentHealth)
        {
            health = currentHealth;
            UpdateHealthDisplay(currentHealth);
        }

        private void UpdateHealthDisplay(int currentHealth)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                if (hearts[i] == null)
                {
                    Debug.LogWarning($"Phần tử tại index {i} là null!");
                }
            }
            
            foreach (Image img in hearts)
            {
                if(img != null)
                    img.sprite = emptyHeart;
            }

            for (int i = 0; i < currentHealth && i < hearts.Count; i++)
            {
                hearts[i].sprite = fullHeart;
            }
        }

        public void UpdateCoins(int totalCoins)
        {

        }
        
        public void OnPauseGame()
        {
            if (pausePanel != null)
            {
                GameManager.Instance.PauseGame();
                pausePanel.SetActive(true);
            }
        }

        public void OnResumeGame()
        {
            GameManager.Instance.ResumeGame();
            pausePanel.SetActive(false);
        }

        public void OnRestartGame()
        {
            GameManager.Instance.RestartGame();
        }

        public void ShowGameOverPanel()
        {
            gameOverPanel.SetActive(true);
        }

        public void HideGameOverPanel()
        {
            gameOverPanel.SetActive(false);
        }

        public void ShowVictoryPanel()
        {
            gameWinPanel.SetActive(true);
        }

        public void HideGameWinPanel()
        {
            gameWinPanel.SetActive(false);
        }
        public void UpdateExpUI(int currentExp, int expToNextLevel, int currentLevel)
        {
            expUiManager.UpdateExpBar(currentExp, expToNextLevel, currentLevel);
        }

        public void UpdateTimerUI(int minutes, int seconds)
        {
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}