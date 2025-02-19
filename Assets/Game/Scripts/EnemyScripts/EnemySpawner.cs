using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.EnemyScripts.Elite_Enemy;
using Game.Scripts.EnemyScripts.RegularEnemy;
using Game.Scripts.Managers;
using Game.Scripts.Map;
using Game.Scripts.PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance {get; private set;}
        
        [Header("Enemy Configurations")] 
        public List<RegularEnemyConfig> enemyConfigs;
        
        [Header("Spawn Settings")]
        public LayerMask whatIsGround;
        
        [Header("Player Reference")]
        public Transform playerTransform; 
        public PlayerHealth playerHealth; 
        
        public LayerMask avoidLayer; 
        public float minDistanceFromPlayer = 5f;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }

        private void OnEnable()
        {
            TimeManager.Instance.OnHalfTimeReached += SpawnEliteEnemy;
        }

        private void OnDisable()
        {
            TimeManager.Instance.OnHalfTimeReached -= SpawnEliteEnemy;
        }

        public void Initialize()
        {
            playerTransform = Player.Instance.playerTransform;
            playerHealth = Player.Instance.playerHealth;

            MapConfig mapConfig = MapManager.Instance.MapConfig;
            
            foreach (var config in mapConfig.regularEnemyConfigs)
            {
                StartCoroutine(SpawnEnemyCoroutine(config));
            }
        }
        
        private void SpawnEnemyFromPool(RegularEnemyConfig config, Vector3 position)
        {
            GameObject enemyObj = EnemyPoolManager.Instance.GetEnemy(config, position, Quaternion.identity);
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Setup(playerTransform, playerHealth, config);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator SpawnEnemyCoroutine(RegularEnemyConfig config)
        {
            while (GameManager.Instance.CurrentState == GameState.Playing)
            {
                yield return new WaitForSeconds(config.spawnRate);

                if (GetValidSpawnPosition(out var spawnPosition))
                {
                    SpawnEnemyFromPool(config, spawnPosition);
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SpawnEliteEnemy()
        {
            MapConfig mapConfig = MapManager.Instance.MapConfig;
            if (mapConfig is null || mapConfig.eliteEnemyConfigs.Count == 0)
            {
                return;
            }

            EliteEnemyConfig config = mapConfig.eliteEnemyConfigs[0];
            if (GetValidSpawnPosition(out var spawnPosition))
            {
                GameObject gameObj = Instantiate(config.enemyPrefab, spawnPosition, Quaternion.identity);
                EliteEnemy enemyScript = gameObj.GetComponent<EliteEnemy>();
                if (enemyScript is not null)
                {
                    enemyScript.Setup(playerTransform, playerHealth, config);
                }
            }
        }

        public void SpawnBossEnemy(BossConfig config)
        {
            if (GetValidSpawnPosition(out var spawnPosition))
            {
                GameObject gameObj = Instantiate(config.enemyPrefab, spawnPosition, Quaternion.identity);
                Enemy enemyScript = GetComponent<Enemy>();
                if (enemyScript)
                {
                    enemyScript.Setup(playerTransform, playerHealth, config);
                }
            }
        }

        private bool GetValidSpawnPosition(out Vector3 spawnPosition)
        {
            Bounds mapBounds = GameManager.Instance.GetBounds();
            for (int i = 0; i < 10; i++) 
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(mapBounds.min.x, mapBounds.max.x),
                    0f, 
                    Random.Range(mapBounds.min.z, mapBounds.max.z)
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

            if (Vector3.Distance(position, playerTransform.position) < minDistanceFromPlayer)
            {
                return false;
            }

            // ReSharper disable once Unity.PreferNonAllocApi
            Collider[] colliders = Physics.OverlapSphere(position, 1f, avoidLayer);
            if (colliders.Length > 0)
            {
                return false;
            }

            return true;
        }
    }
}
