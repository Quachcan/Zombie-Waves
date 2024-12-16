using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Configurations")] 
        public List<EnemyConfigSo> enemyConfigs;
        
        [Header("Spawn Settings")]
        public LayerMask whatIsGround; 
        
        public LayerMask avoidLayer; 
        public float minDistanceFromPlayer = 5f; 
        public Transform player; 

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

        private IEnumerator SpawnEnemyRoutine(EnemyConfigSo config)
        {
            while (true)
            {
                yield return new WaitForSeconds(config.spawnRate);

                Vector3 spawnPosition = GetValidSpawnPosition();
                if (spawnPosition != Vector3.zero)
                {
                    GameObject enemy = Instantiate(config.enemyPrefab, spawnPosition, Quaternion.identity);
                    
                    var enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        //enemyScript._maxHealth = config.health;
                    }
                }
            }
        }

        private Vector3 GetValidSpawnPosition()
        {
            for (int i = 0; i < 10; i++) // Thử tối đa 10 lần để tìm vị trí hợp lệ
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-50f, 50f), // Điều chỉnh phạm vi bản đồ
                    0f, 
                    Random.Range(-50f, 50f)
                );

                if (IsValidPosition(randomPosition))
                {
                    return randomPosition;
                }
            }
            
            return Vector3.zero;
        }

        private bool IsValidPosition(Vector3 position)
        {
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
