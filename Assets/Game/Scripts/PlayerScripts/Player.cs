using System;
using System.Collections.Generic;
using DataSystem;
using Game.Scripts.BuffSystem;
using Game.Scripts.Camera;
using Game.Scripts.Managers;
using Managers;
using PlayerScripts;
using UnityEngine;

namespace Game.Scripts.PlayerScripts
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
        public int expToNextLevel = 10;
        public static event Action<int> OnLevelUp;
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
            if (CameraFollow.Instance != null)
            {
                CameraFollow.Instance.SetTarget(transform);
            }
        }
        
        public void AddExperience(int amount)
        {  
            currentExp += amount;

            if (currentExp >= expToNextLevel)
            {
                LevelUp();
            }
            NotifyExpChange();
        }

        private void LevelUp() 
        {
            currentLevel++;
            currentExp -= expToNextLevel;
            expToNextLevel += 5;
            Debug.Log($"Level Up! New Level: {currentLevel}");
            OnLevelUp?.Invoke(currentLevel);
            List<BuffConfig> selectedBuff = BuffManager.Instance.GetRandomBuffs();
            foreach (BuffConfig buff in selectedBuff)
            {
                BuffManager.Instance.ApplyBuff(buff);
            }
            NotifyExpChange();
        }

        private void NotifyExpChange()
        {
            GameManager.Instance.UpdatePlayerExp(currentExp, expToNextLevel, currentLevel);
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
                expToNextLevel = data.expToNextLevel;
                
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2] );
                transform.position = position;
            }
        }
    }
}
