using System.Collections.Generic;
using BuffSystem;
using UnityEngine;

namespace Managers
{
    public class BuffManager : MonoBehaviour
    {
        public static BuffManager Instance { get; private set; }
        
        private List<Buff> activeBuffs = new List<Buff>();

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

        public void ApplyBuff(Buff buff)
        {
            activeBuffs.Add(buff);
            
            buff.Apply();
            Debug.Log($"Applied Buff: {buff.BuffName}");
        }
        
    }
}