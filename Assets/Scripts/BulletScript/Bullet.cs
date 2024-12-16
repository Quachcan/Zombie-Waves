using System.Collections;
using EnemyScripts;
using UnityEngine;

namespace BulletScript
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private int damage;

        public void Initialize(Transform target, int damage)
        {
            this.target = target;
            this.damage = damage;
        }
        
        public IEnumerator BulletCoroutineLifeTime(float lifeTime)
        {
            yield return new WaitForSeconds(0.5f);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
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
