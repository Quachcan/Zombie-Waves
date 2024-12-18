using System.Collections;
using UnityEngine;

namespace BulletScript
{
    public class Bullet : MonoBehaviour
    {
        [Header("Bullet Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float lifetime = 3f;
        
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
            transform.position += direction * (speed * Time.deltaTime);
        }

        private IEnumerator BulletCoroutineLifeTime()
        {
            yield return new WaitForSeconds(lifetime);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyScripts.Enemy enemy = other.GetComponent<EnemyScripts.Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }

                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}