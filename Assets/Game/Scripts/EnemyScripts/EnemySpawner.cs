using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Managers;
using Game.Scripts.PlayerScripts;
using Managers;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance {get; private set;}
        
        [Header("Enemy Configurations")] 
        public List<EnemyConfig> enemyConfigs;
        
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

        public void Initialize()
        {
            playerTransform = Player.Instance.playerTransform;
            playerHealth = Player.Instance.playerHealth;
            
            if (enemyConfigs == null || enemyConfigs.Count == 0)
            {
                return;
            }
            
            foreach (var config in enemyConfigs)
            {
                StartCoroutine(SpawnEnemyRoutine(config));
            }
        }

        // private void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
        // {
        //     GameObject enemyObj = Instantiate(enemyPrefab, position, Quaternion.identity);
        //     Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        //
        //     if (enemyScript != null)
        //     {
        //         enemyScript.Setup(playerTransform, playerHealth);
        //     }
        //     
        //     // if (enemyScript != null)
        //     // {
        //     //     enemyScript.Init();
        //     //     enemyScript.Initialize(playerTransform, playerHealth);
        //     // }
        //     //
        //     // var enemyMovement = enemyObj.GetComponent<EnemyMovement>();
        //     // if (enemyMovement != null)
        //     // {
        //     //     enemyMovement.Initialize();
        //     //     enemyMovement.SetPlayer(playerTransform);
        //     // }
        // }

        private void SpawnEnemyFromPool(EnemyConfig config, Vector3 position)
        {
            GameObject enemyObj = EnemyPoolManager.Instance.GetEnemy(config, position, Quaternion.identity);
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Setup(playerTransform, playerHealth, config);
            }
        }

        private IEnumerator SpawnEnemyRoutine(EnemyConfig config)
        {
            while (GameManager.Instance.CurrentState == GameState.Playing)
            {
                yield return new WaitForSeconds(config.spawnRate);

                Vector3 spawnPosition;
                if (GetValidSpawnPosition(out spawnPosition))
                {
                    //SpawnEnemy(config.enemyPrefab, spawnPosition);
                    SpawnEnemyFromPool(config, spawnPosition);
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

            Collider[] colliders = Physics.OverlapSphere(position, 1f, avoidLayer);
            if (colliders.Length > 0)
            {
                return false;
            }

            return true;
        }
    }
}
