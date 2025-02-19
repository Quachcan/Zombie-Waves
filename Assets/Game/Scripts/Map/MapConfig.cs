using System.Collections.Generic;
using Game.Scripts.EnemyScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Map
{
    [CreateAssetMenu(fileName = "Map", menuName = "New Map/MapConfig")]
    public class MapConfig : ScriptableObject
    {
        public List<RegularEnemyConfig> regularEnemyConfigs;
        public List<EliteEnemyConfig> eliteEnemyConfigs;
        public List<BossConfig> bossConfigs;
    }
}
