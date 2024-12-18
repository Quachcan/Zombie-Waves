using PlayerScripts;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        
        public Transform PlayerTransform { get; private set; }
        public PlayerHealth PlayerHealth {get; private set;}

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

        public void RegisterPlayer(GameObject player)
        {
            if (player == null)
            {
                return;
            }
            PlayerTransform = player.transform;
            PlayerHealth = player.GetComponent<PlayerHealth>();
        }
    }
}
