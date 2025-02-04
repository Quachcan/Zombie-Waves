using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public float distance;
        
        public Transform player;
        private NavMeshAgent agent;
        
        private readonly float updateInterval = 0.2f;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        public void SetPlayer(Transform playerTransform)
        {
            player = playerTransform;
        }

        public void Initialize()
        {
            InvokeRepeating(nameof(UpdateDestination), 0f, updateInterval);
        }

        private void Update()
        {
            if (player == null) return;
            distance = Vector3.Distance(transform.position, player.transform.position);
        }

        private void UpdateDestination()
        {
            if (player != null)
            {
                agent.SetDestination(player.position);
            }
        }

    }
}
