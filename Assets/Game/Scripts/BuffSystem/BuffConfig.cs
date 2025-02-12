using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.BuffSystem
{
    [CreateAssetMenu(fileName = "NewBuff", menuName = "BuffSystem/BuffConfig")]
    public class BuffConfig : ScriptableObject
    {
        public string buffName;
        public int buffValue;
        public BuffType buffType;
        public int buffLevel;
        public Sprite buffIcon;
    }

    public enum BuffType
    {
        Attack,
        FireRate,
        Movement,
        Health
    }
}

