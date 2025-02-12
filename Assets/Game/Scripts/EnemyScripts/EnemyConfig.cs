using UnityEngine;

namespace Game.Scripts.EnemyScripts
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "New Enemy/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public GameObject enemyPrefab; 
        public float spawnRate = 1f; 
        public int health = 10; 
        public int damage = 1;
    }
}