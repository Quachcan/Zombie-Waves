using Game.Scripts.EnemyScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.EnemyScripts
{
    [CreateAssetMenu(fileName = "Boss", menuName = "New Enemy/BossEnemyConfig")]
    public class BossConfig : BaseEnemyConfig
    {
        public string bossName = "Boss";
        public int extraDamage = 3;
    }
}


