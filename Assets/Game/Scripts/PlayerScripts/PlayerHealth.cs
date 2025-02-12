using System;
using System.Collections;
using Game.Scripts.Managers;
using UnityEngine;

namespace Game.Scripts.PlayerScripts
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private int currentHealth;
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        private float immunityDuration;
        [SerializeField]
        private bool isImmuneToDamage;
        
        public event Action<int> OnMaxHealthChanged;
        public event Action<int> OnCurrentHealthChanged;

        public int MaxHealth => maxHealth;

        public int CurrentHealth => currentHealth;

        // Start is called before the first frame update
        public void Initialize()
        {
            currentHealth = maxHealth;
            isImmuneToDamage = false;
            OnMaxHealthChanged?.Invoke(maxHealth);
            OnCurrentHealthChanged?.Invoke(currentHealth);
        }

        public void TakeDamage(int damage)
        {
            if (currentHealth < 0) return;

            if (isImmuneToDamage)
            {
                return;
            }
            
            currentHealth -= damage;
            OnCurrentHealthChanged?.Invoke(currentHealth);
            
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(ImmuneToDamage());
            }
        }

        private void Die()
        {
            GameManager.Instance.GameOver();
        }

        private IEnumerator ImmuneToDamage()
        {
            isImmuneToDamage = true;
            
            yield return new WaitForSeconds(immunityDuration);
            
            isImmuneToDamage = false;
        }

        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;
            currentHealth = maxHealth;
            
            OnMaxHealthChanged?.Invoke(maxHealth);
            OnCurrentHealthChanged?.Invoke(currentHealth);
        }
    }
}