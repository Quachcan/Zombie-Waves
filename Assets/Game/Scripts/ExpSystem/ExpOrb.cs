using PlayerScripts;
using UnityEngine;
using System.Collections;
using Game.Scripts.PlayerScripts;

namespace ExpSystem
{
    public class ExpOrb : MonoBehaviour
    {

        [Header("Settings")]
        public float moveSpeed = 5f;
        public int expValue = 1;
        public float activationDistance = 5f;
        
        [SerializeField]
        private Transform player;
        private bool isActivated;
        
        public void Initialize()
        {
            StartCoroutine(WaitForPlayer());
        }
        
        private IEnumerator WaitForPlayer()
        {
            while (Player.Instance == null)
            {
                yield return null; 
            }

            player = Player.Instance.playerTransform;
        }

        // Update is called once per frame
        void Update()
        {
            if(player == null) return;
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            if (!isActivated && distanceToPlayer < activationDistance)
            {
                isActivated = true;
            }

            if (isActivated)
            {
                MoveTowardsPlayer();
            }
        }

        private void MoveTowardsPlayer()
        {
            transform.position = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player.Instance.AddExperience(expValue);
                
                Destroy(gameObject);
            }
        }
    }
}
