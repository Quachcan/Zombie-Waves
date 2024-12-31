using JetBrains.Annotations;
using UnityEngine;

namespace BuffSystem
{
    public abstract class Buff
    {
        public readonly string BuffName; 

        protected PlayerScripts.Player Player;

        public Buff(string name, float duration)
        {
            this.BuffName = name;
            this.Player = PlayerScripts.Player.Instance;
        }
    
        public abstract void ApplyBuff(GameObject target);

        public abstract void RemoveBuff(GameObject target);  

    }
}