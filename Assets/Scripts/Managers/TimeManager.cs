using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        private bool isTimeRunning;
        [SerializeField]
        private float gameDuration = 300f;
        [SerializeField]
        private float currentTime;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            ResetTimer();
        }

        private void Update()
        {
            if (isTimeRunning)
            {
                UpdateTimer();
            }
        }

        public void StartTimer()
        {
            isTimeRunning = true;
        }

        private void StopTimer()
        {
            isTimeRunning = false;
        }

        private void UpdateTimer()
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0f;
                StopTimer();
                GameManager.Instance.OnTimeUp();
            }
            NotifyUIManager();
        }

        private void NotifyUIManager()
        {
            if (UIManager.Instance == null)
            {
                return;
            }
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            UIManager.Instance.UpdateTimerUI(minutes, seconds);
        }

        private void ResetTimer()
        {
            currentTime = gameDuration;
            StopTimer();
            NotifyUIManager();
        }
    }
}
