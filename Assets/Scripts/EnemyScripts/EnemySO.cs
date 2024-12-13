using UnityEngine;

namespace EnemyScripts
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "New Enemy/EnemyConfig")]
    public class EnemyConfigSO : ScriptableObject
    {
        public GameObject enemyPrefab; 
        public float spawnRate = 1f; 
        public int health = 100; 
        public int damage = 10;
        public int speed = 2;
    }
}