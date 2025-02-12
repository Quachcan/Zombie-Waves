using System.Collections;
using UnityEngine;
using Game.Scripts.EnemyScripts;

namespace Game.Scripts.BulletScript
{
    public class Bullet : MonoBehaviour
    {
        [Header("Bullet Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private LayerMask whatIsEnemy;
        
        [SerializeField] private bool hasHit;
        private RaycastHit lastHit;
        private Vector3 direction;


        public void Initialize(Vector3 shootDirection, int bulletDamage)
        {
            this.direction = shootDirection.normalized;
            this.damage = bulletDamage;

            StopAllCoroutines();
            StartCoroutine(BulletCoroutineLifeTime());
        }

        private void Update()
        {
            float distance = speed * Time.deltaTime;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, whatIsEnemy))
            {
                lastHit = hit;
                hasHit = true;
                OnHit(hit);
            }

            transform.position += direction * distance;
        }


        private void OnHit(RaycastHit hit)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            ReturnToPool();
        }
        private IEnumerator BulletCoroutineLifeTime()
        {
            yield return new WaitForSeconds(lifetime);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            hasHit = false;
            BulletPool.Instance.ReturnBullet(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (!hasHit)
            {
                Gizmos.DrawRay(transform.position, direction * speed * Time.deltaTime);
            }
            else
            {
                Gizmos.DrawRay(transform.position, direction * Vector3.Distance(transform.position, lastHit.point));
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(lastHit.point, 0.1f);
            }
        }
    }
}