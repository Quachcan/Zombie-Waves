using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public float distance;
        
        public Transform player;
        private NavMeshAgent _agent;
        
        private float _updateInterval = 0.2f;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        public void SetPlayer(Transform playerTransform)
        {
            player = playerTransform;
        }
        private void Start()
        {
            InvokeRepeating(nameof(UpdateDestination), 0f, _updateInterval);
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
                _agent.SetDestination(player.position);
            }
        }

    }
}
