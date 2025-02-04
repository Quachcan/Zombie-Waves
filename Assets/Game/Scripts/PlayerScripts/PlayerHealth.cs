using Managers;
using UnityEngine;
using System.Collections;

namespace PlayerScripts
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
        // Start is called before the first frame update
        public void Initialize()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (currentHealth < 0) return;

            if (isImmuneToDamage)
            {
                return;
            }
            
            currentHealth -= damage;
            GameManager.Instance.OnPlayerTakeDamage(currentHealth);

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
    }
}