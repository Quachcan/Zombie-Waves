using System;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        public static Player Instance {get; private set;}
        
        public Transform playerTransform;
        public PlayerHealth playerHealth;
        public PlayerMovement playerMovement;
        public PlayerCombat playerCombat;
        
        
        
        [Header("Leveling Settings")]
        public int currentLevel = 1;
        public int currentExp = 0;
        public int expTotNextLevel = 10;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(this);
            
        }

        public void Initialize()
        {
            playerTransform = transform;
            playerHealth = GetComponent<PlayerHealth>();
            playerMovement = GetComponent<PlayerMovement>();
            playerCombat = GetComponent<PlayerCombat>();
            
            playerCombat.Initialize();
            playerMovement.Initialize();
            playerHealth.Initialize();
        }

        public void AddExperience(int amount)
        {  
            currentExp += amount;
            Debug.Log($"Player gained {amount} EXP! Current EXP: {currentExp}");

            if (currentExp >= expTotNextLevel)
            {
                LevelUp();
            }
            NotifyExpChange();
        }

        private void LevelUp() 
        {
            currentLevel++;
            currentExp -= expTotNextLevel;
            expTotNextLevel += 10;
            Debug.Log($"Level Up! New Level: {currentLevel}");
            NotifyExpChange();
        }

        private void NotifyExpChange()
        {
            GameManager.Instance.UpdatePlayerExp(currentExp, expTotNextLevel, currentLevel);
        }
    }
}
