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
    
}