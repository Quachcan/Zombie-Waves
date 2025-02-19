using System.Collections.Generic;
using System.IO.Pipes;
using Game.Scripts.EnemyScripts;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class EnemyPoolManager : MonoBehaviour
    {
        public static EnemyPoolManager Instance { get; private set; }
        
        public int poolSize;
        
        private readonly Dictionary<BaseEnemyConfig, Stack<GameObject>> pools = new Dictionary<BaseEnemyConfig, Stack<GameObject>>();

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetPrefab(BaseEnemyConfig config)
        {
            if (config.enemyPrefab is null)
            {
                if(string.IsNullOrEmpty(config.resourcePath))
                    Debug.LogError("Resource path is not set in config" + config.name);
            }
            
            config.enemyPrefab = Resources.Load<GameObject>(config.resourcePath);
            if (config.enemyPrefab is null)
            {
                Debug.LogError($"{config.resourcePath} not found");
            }

            return config.enemyPrefab;
        }

        public GameObject GetEnemy(BaseEnemyConfig config, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = GetPrefab(config);
            if(prefab is null) return null;
            
            if (!pools.ContainsKey(config))
            {
                pools.Add(config, new Stack<GameObject>());
                for (int i = 0; i < poolSize; i++)
                {
                    GameObject enemyObj = Instantiate(config.enemyPrefab, transform);
                    enemyObj.SetActive(false);
                    pools[config].Push(enemyObj);
                }
            }
            
            GameObject enemyFromPool;
            if (pools[config].Count > 0)
            {
                enemyFromPool = pools[config].Pop();
                enemyFromPool.SetActive(true);
            }
            else
            {
                enemyFromPool = Instantiate(config.enemyPrefab, transform);
            }
                    
            enemyFromPool.transform.position = position;
            enemyFromPool.transform.rotation = rotation;
            return enemyFromPool;
        }

        public void ReturnEnemyToPool(BaseEnemyConfig config, GameObject enemyObj)
        {
            enemyObj.SetActive(false);
            if (!pools.ContainsKey(config))
            {
                pools[config] = new Stack<GameObject>();
            }
            pools[config].Push(enemyObj);
        }
    }
}
