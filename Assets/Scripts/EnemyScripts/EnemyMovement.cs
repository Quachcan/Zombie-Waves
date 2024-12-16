using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public float distance;
        
        public Transform player;
        private NavMeshAgent _agent;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            _agent.SetDestination(player.position);
        }
    }
}
