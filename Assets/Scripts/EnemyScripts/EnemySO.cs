using UnityEngine;

namespace EnemyScripts
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "New Enemy/EnemyConfig")]
    public class EnemyConfigSo : ScriptableObject
    {
        public GameObject enemyPrefab; 
        public float spawnRate = 1f; 
        public int health = 10; 
        public int damage = 1;
    }
}