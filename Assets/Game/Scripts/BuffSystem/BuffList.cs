using System.Collections.Generic;

namespace Game.Scripts.BuffSystem
{
    [System.Serializable]
    public class BuffList
    {
        public BuffType buffType;
        public List<BuffConfig> buffConfigs;
    }
}
