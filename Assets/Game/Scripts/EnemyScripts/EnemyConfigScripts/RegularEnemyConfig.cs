using Game.Scripts.EnemyScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.EnemyScripts
{
    [CreateAssetMenu(fileName = "RegularEnemy", menuName = "New Enemy/RegularEnemyConfig")]
    public class RegularEnemyConfig : BaseEnemyConfig
    {
        public float spawnRate = 1f;
    }
}


