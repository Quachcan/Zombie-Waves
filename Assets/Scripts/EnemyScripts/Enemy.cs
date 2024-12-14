using System;
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
            Debug.Log($"{name} took {damage} damage, remaining health: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{name} died.");
            Destroy(gameObject);
        }
    }
}