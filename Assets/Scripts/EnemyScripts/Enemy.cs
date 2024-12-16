using System;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth;
        private int _currentHealth;
        

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Update()
        {
            
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            PlayerCombat playerCombat = FindObjectOfType<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.RemoveEnemy(this);
            }
            Destroy(gameObject);
        }
    }
}