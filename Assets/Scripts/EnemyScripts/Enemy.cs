using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour
    {
        private int _health;

        public float distance;
        
        public Transform player;
        private NavMeshAgent agent;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponentInChildren<NavMeshAgent>();
        }

        private void Update()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            agent.SetDestination(player.position);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            Debug.Log($"{name} took {damage} damage, remaining health: {_health}");

            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{name} died.");
            Destroy(gameObject); // Xóa Enemy khi chết
        }
    }
}