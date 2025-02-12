using System.Collections.Generic;
using Game.Scripts.EnemyScripts;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class EnemyPoolManager : MonoBehaviour
    {
        public static EnemyPoolManager Instance { get; private set; }
        
        public int poolSize;
        
        private Dictionary<EnemyConfig, Stack<GameObject>> pools = new Dictionary<EnemyConfig, Stack<GameObject>>();

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetEnemy(EnemyConfig config, Vector3 position, Quaternion rotation)
        {
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

        public void ReturnEnemyToPool(EnemyConfig config, GameObject enemyObj)
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
