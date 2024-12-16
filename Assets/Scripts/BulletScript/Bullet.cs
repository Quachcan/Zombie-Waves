using System.Collections;
using UnityEngine;

namespace BulletScript
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField] private int damage;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void ReturnToPool()
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        
        public IEnumerator BulletCoroutineLifeTime(float lifeTime)
        {
            yield return new WaitForSeconds(0.5f);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == target)
            {
                Debug.Log("Bullet Hit: " + target.name);
                StopAllCoroutines();
                BulletPool.Instance.ReturnBullet(gameObject);
            }
        }
    
    
    }
}
