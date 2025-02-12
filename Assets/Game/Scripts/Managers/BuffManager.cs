using System;
using System.Collections.Generic;
using BuffSystem;
using Game.Scripts.BuffSystem;
using Game.Scripts.PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Managers
{
    public class BuffManager : MonoBehaviour
    {
        public static BuffManager Instance { get; private set; }
        
       // [Header("Buff Lists")]
       // [SerializeField]
       // private List<BuffConfig> attackBuffs;
       // [SerializeField]
       // private List<BuffConfig> fireRateBuffs;
       // [SerializeField]
       // private List<BuffConfig> movementSpeedBuffs;
       // [SerializeField]
       // private List<BuffConfig> healthBuffs;
       
       [SerializeField] 
       private List<BuffList> buffLists;
       
       public static event Action<List<BuffConfig>> OnBuffSelection;
       
       private Dictionary<BuffType, Action<BuffConfig>> buffActions;
       
       private Dictionary<BuffType, List<BuffConfig>> buffDictionary;
       
       private readonly Dictionary<BuffType, int> buffLevels = new Dictionary<BuffType, int>();
       
       private void Awake()
       {
           if (Instance != null && Instance != this)
           {
               Destroy(this.gameObject);
               return;
           }
           Instance = this;
           
           buffDictionary = new Dictionary<BuffType, List<BuffConfig>>();
           foreach (BuffList buff in buffLists)
           {
               buffDictionary[buff.buffType] = buff.buffConfigs;
           }

           buffActions = new Dictionary<BuffType, Action<BuffConfig>>
           {
               { BuffType.Attack, buff => Player.Instance.playerCombat.IncreaseDamage(buff.buffValue) },
               { BuffType.FireRate, buff => Player.Instance.playerCombat.IncreaseFireRate(buff.buffValue)},
               { BuffType.Movement, buff => Player.Instance.playerMovement.IncreaseMovementSpeed(buff.buffValue)},
               { BuffType.Health, buff => Player.Instance.playerHealth.IncreaseMaxHealth(buff.buffValue)}
           };
       }

       private void OnEnable()
       {
           Player.OnLevelUp += SelectRandomBuffs;
       }

       private void OnDisable()
       {
           Player.OnLevelUp -= SelectRandomBuffs;
       }

       private void SelectRandomBuffs(int level)
       {
           List<BuffConfig> selectedBuffs = GetRandomBuffs();
           OnBuffSelection?.Invoke(selectedBuffs);
       }

       public List<BuffConfig> GetRandomBuffs()
       {
           
           // if (attackBuffs.Count > 0) allBuffs.Add(GetNextBuff(attackBuffs, BuffType.Attack));
           // if (fireRateBuffs.Count > 0) allBuffs.Add(GetNextBuff(fireRateBuffs, BuffType.FireRate));
           // if (movementSpeedBuffs.Count > 0) allBuffs.Add(GetNextBuff(movementSpeedBuffs, BuffType.MovementSpeed));
           // if (healthBuffs.Count > 0) allBuffs.Add(GetNextBuff(healthBuffs, BuffType.Health));
           
           List<BuffConfig> allBuffs = new List<BuffConfig>();
           foreach (var kvp in buffDictionary)
           {
               BuffConfig buff = GetNextBuff(kvp.Value, kvp.Key);
               if (buff != null)
               {
                   allBuffs.Add(buff);
               }
           }
           
           List<BuffConfig> selectedBuffs = new List<BuffConfig>();
           while (selectedBuffs.Count < 3 && allBuffs.Count > 0)
           {
               int randomIndex = Random.Range(0, allBuffs.Count);
               selectedBuffs.Add(allBuffs[randomIndex]);
               allBuffs.RemoveAt(randomIndex);
           }
           
           return selectedBuffs;
       }

       private BuffConfig GetNextBuff(List<BuffConfig> buffList, BuffType buffType)
       {
           buffLevels.TryGetValue(buffType, out int currentLevel);
           
           List<BuffConfig> availableBuffs = buffList.FindAll(buff => buff.buffLevel == currentLevel + 1);
           if (availableBuffs.Count == 0) return null;
           return availableBuffs[Random.Range(0, availableBuffs.Count)];
       }

       public void ApplyBuff(BuffConfig buff)
       {
           if(buffLevels.ContainsKey(buff.buffType))
               buffLevels[buff.buffType]++;
           else
               buffLevels[buff.buffType] = 1;

           if (buffActions.TryGetValue(buff.buffType, out Action<BuffConfig> action))
           {
               action.Invoke(buff);
           }
           else
           {
               Debug.LogWarning($"Cannot find any action for buff type {buff.buffType}");
           }
       }
    }
}