using UnityEngine;
using UnityEngine.AI;

namespace EnemyScripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public float distance;
        
        public Transform player;
        private NavMeshAgent agent;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            agent.SetDestination(player.position);
        }
    }
}
