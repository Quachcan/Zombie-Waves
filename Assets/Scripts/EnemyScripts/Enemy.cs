using System;
using PlayerScripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private EnemyMovement enemyMovement;
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        private int currentHealth;

        private Transform playerTransform;
        private PlayerHealth playerHealth;

        public void Initialize(Transform player, PlayerHealth health)
        {
            playerTransform = player;
            playerHealth = health;
        }
        private void Awake()
        {
            currentHealth = maxHealth;
            enemyMovement = GetComponent<EnemyMovement>();
        }

        private void OnEnable()
        {
            PlayerCombat.Instance.RegisterEnemy(this);
        }

        private void OnDisable()
        {
            PlayerCombat.Instance.UnregisterEnemy(this);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            PlayerCombat.Instance.UnregisterEnemy(this);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && enemyMovement.distance <= 1.5f)
            {
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(1);
                }
            }
        }
    }
}