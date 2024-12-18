using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Configurations")] 
        public List<EnemyConfigSo> enemyConfigs;
        
        [Header("Spawn Settings")]
        public LayerMask whatIsGround;
        
        [Header("Player Reference")]
        public Transform player; 
        public PlayerHealth playerHealth; 
        
        public LayerMask avoidLayer; 
        public float minDistanceFromPlayer = 5f; 

        private void Awake()
        {
            player = Managers.PlayerManager.Instance.PlayerTransform;
            playerHealth = Managers.PlayerManager.Instance.PlayerHealth;
        }
        
        private void Start()
        {
            if (enemyConfigs == null || enemyConfigs.Count == 0)
            {
                Debug.LogError("No enemy configurations assigned!");
                return;
            }
            
            foreach (var config in enemyConfigs)
            {
                StartCoroutine(SpawnEnemyRoutine(config));
            }
        }

        private void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, position, Quaternion.identity);
            var enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(player, playerHealth);
            }
            
            var enemyMovement = enemyObj.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.SetPlayer(player);
            }
        }

        private IEnumerator SpawnEnemyRoutine(EnemyConfigSo config)
        {
            while (true)
            {
                yield return new WaitForSeconds(config.spawnRate);

                Vector3 spawnPosition;
                if (GetValidSpawnPosition(out spawnPosition))
                {
                    SpawnEnemy(config.enemyPrefab, spawnPosition);
                }
            }
        }

        private bool GetValidSpawnPosition(out Vector3 spawnPosition)
        {
            for (int i = 0; i < 10; i++) 
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-50f, 50f),
                    0f, 
                    Random.Range(-50f, 50f)
                );

                if (IsValidPosition(randomPosition))
                {
                    spawnPosition = randomPosition;
                    return true;
                }
            }
            
            spawnPosition = Vector3.zero;
            return false;

        }

        private bool IsValidPosition(Vector3 position)
        {
            if (position == Vector3.zero)
            {
                return false;
            }
            
            if (!Physics.Raycast(position + Vector3.up * 10f, Vector3.down, 20f, whatIsGround))
            {
                return false;
            }
            
            if (Vector3.Distance(position, player.position) < minDistanceFromPlayer)
            {
                return false;
            }
            
            Collider[] colliders = Physics.OverlapSphere(position, 1f, avoidLayer);
            if (colliders.Length > 0)
            {
                return false;
            }

            return true; 
        }
    }
}
