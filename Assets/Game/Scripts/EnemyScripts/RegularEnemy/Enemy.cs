using System;
using Game.Scripts.Managers;
using Game.Scripts.PlayerScripts;
using PlayerScripts;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.EnemyScripts.RegularEnemy
{
    public class Enemy : MonoBehaviour
    {
        public static event Action<Vector3> OnEnemyDeath;
        
        [SerializeField]
        private EnemyMovement enemyMovement;
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        private int currentHealth;

        private Transform playerTransform;
        private PlayerHealth playerHealth;
        private BaseEnemyConfig baseEnemyConfig;

        public Transform hitPoint;

        public void Initialize(Transform player, PlayerHealth health)
        {
            playerTransform = player;
            playerHealth = health;
        }
        public void Init()
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
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Setup(Transform player, PlayerHealth health, BaseEnemyConfig config)
        {
            baseEnemyConfig = config;
            Init();
            Initialize(player, health);

            EnemyMovement movement = GetComponent<EnemyMovement>();
            if (movement != null)
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = true;
                }
                movement.Initialize();
                movement.SetPlayer(player);
            }
        }

        public void TakeDamage(int damage)
        {
            if (currentHealth <= 0) return;
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            if (enemyMovement != null)
            {
                enemyMovement.CancelInvoke();
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                }
            }
            PlayerCombat.Instance.UnregisterEnemy(this);
            OnEnemyDeath?.Invoke(transform.position);
            
            if(baseEnemyConfig != null)
                EnemyPoolManager.Instance.ReturnEnemyToPool(baseEnemyConfig, gameObject);
            else
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
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