using System;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        public event Action OnHalfTimeReached;
        public event Action OnThirdTimeReached;
        
        private bool isTimeRunning;
        [SerializeField] private float gameDuration = 60f;
        [SerializeField] private float currentTime;
        
        private bool halfTimeReached;
        private bool thirdTimeReached;
        private void Awake()
        {
            Instance = this;
            
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

            if (!halfTimeReached && currentTime <= gameDuration / 2f)
            {
                halfTimeReached = true;
                OnHalfTimeReached?.Invoke();
            }

            if (!thirdTimeReached && currentTime <= gameDuration / 3f)
            {
                thirdTimeReached = true;
                OnThirdTimeReached?.Invoke();
            }

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
            if (UIManager.Instance is null)
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
            halfTimeReached = false;
            thirdTimeReached = false;
            StopTimer();
            NotifyUIManager();
        }
    }
}
