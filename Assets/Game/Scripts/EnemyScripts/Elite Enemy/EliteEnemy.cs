using System;
using Game.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.EnemyScripts.Elite_Enemy
{
    public class EliteEnemy : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        [SerializeField] private int movementSpeed;
        [SerializeField] private int attackDamage;
        private bool isDead;
        
        private EliteEnemyConfig config;
        private NavMeshAgent agent;
        private Transform target;

        public void Setup(Transform playerTransform, PlayerHealth playerHealth, EliteEnemyConfig config)
        {
            this.config = config;
            maxHealth = config.health;
            currentHealth = maxHealth;
            attackDamage = config.damage;
            target = playerTransform;
            
            agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = config.moveSpeed;
            }
        }

        private void MoveTo(Vector3 destination)
        {
            if (agent)
                agent.SetDestination(destination);
        }

        private void Update()
        {
            if (target is not null && agent is not null && agent.enabled)
            {
                MoveTo(target.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }

        public void TakeDamage(int amount)
        {
            if(isDead) return;
            
            currentHealth -= amount;
            if(currentHealth <= 0)
            {
                Die();
                isDead = true;
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
