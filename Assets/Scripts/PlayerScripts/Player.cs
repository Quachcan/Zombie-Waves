using System;
using DataSystem;
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
            RegisterComponents();
        }

        private void RegisterComponents()
        {
            if (TopDownCameraFollow.instance != null)
            {
                TopDownCameraFollow.instance.SetTarget(transform);
            }
        }
        
        public void AddExperience(int amount)
        {  
            currentExp += amount;
//            Debug.Log($"Player gained {amount} EXP! Current EXP: {currentExp}");

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
            expTotNextLevel += 5;
            Debug.Log($"Level Up! New Level: {currentLevel}");
            NotifyExpChange();
        }

        private void NotifyExpChange()
        {
            GameManager.Instance.UpdatePlayerExp(currentExp, expTotNextLevel, currentLevel);
        }

        public void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }

        public void LoadPlayer()
        {
            PlayerData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                currentLevel = data.currentLevel;
                currentExp = data.currentExp;
                expTotNextLevel = data.expToNextLevel;
                
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2] );
                transform.position = position;
            }
        }
    }
}
