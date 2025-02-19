using UnityEngine;

namespace Game.Scripts.EnemyScripts
{

    public class BaseEnemyConfig : ScriptableObject
    {
        public GameObject enemyPrefab;
        public string resourcePath;
        public int health = 10; 
        public int damage = 1;
        public float moveSpeed;
    }
    
    
    [CreateAssetMenu(fileName = "RegularEnemyConfig", menuName = "New Enemy/RegularEnemyConfig")]
    public class RegularEnemyConfig : BaseEnemyConfig
    {
        public float spawnRate = 1f; 
    }

    [CreateAssetMenu(fileName = "EliteEnemy", menuName = "New Enemy/EliteEnemyConfig")]
    public class EliteEnemyConfig : BaseEnemyConfig
    {
        public string eliteName = "Elite";
        public int extraDamage = 2;
    }

    [CreateAssetMenu(fileName = "Boss", menuName = "New Enemy/BossEnemyConfig")]
    public class BossConfig : BaseEnemyConfig
    {
        public string bossName = "Boss";
        public int extraDamage = 3;
    }
}