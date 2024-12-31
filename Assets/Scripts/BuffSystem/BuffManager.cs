using System;
using System.Collections.Generic;
using BuffSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class BuffManager : MonoBehaviour
    {
        public static BuffManager Instance { get; private set; }
        [SerializeField]
        private Dictionary<Type, IBuffHandler> buffHandler = new Dictionary<Type, IBuffHandler>();
        [SerializeField]
        private List<BuffConfig> activeBuffs = new List<BuffConfig>();
        private GameObject player;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize(GameObject player)
        {
            this.player = player;
        }


        public void RegisterBuff()
        {
            buffHandler[typeof(AttackBuff)] = new AttackBuff();
            buffHandler[typeof(MovementSpeedBuff)] = new MovementSpeedBuff();
            buffHandler[typeof(FireRateBuff)] = new FireRateBuff();
        }

        public void ApplyBuff(BuffConfig buffConfig)
        {
           if (buffHandler.TryGetValue(buffConfig.BuffType, out var handler))
            {
                handler.Apply(player, buffConfig.value);
                activeBuffs.Add(buffConfig);
                Debug.Log($"{buffConfig.BuffName} applied");
            }
           else
            {
                Debug.LogWarning($"No buff registed for BuffType: {buffConfig.BuffType}");
            }
        }

        public void RemoveAllBuff()
        {
            foreach (var buff in activeBuffs)
            {
                if(buffHandler.TryGetValue(buff.BuffType, out var handler))
                {
                    handler.Remove(player, buff.value);
                }
            }
            
            activeBuffs.Clear();
        }
       
    }
}