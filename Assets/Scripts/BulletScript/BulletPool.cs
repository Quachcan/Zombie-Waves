using System.Collections.Generic;
using UnityEngine;

namespace BulletScript
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance { get; private set; }
        public GameObject bulletPrefab;
        public int bulletPoolSize;
    
        private readonly Queue<GameObject> _bulletPool = new Queue<GameObject>();
        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Debug.Log("Pool created");
            }
            else
            {
                Destroy(gameObject);
            }
            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < bulletPoolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform);
                bullet.SetActive(false);
                _bulletPool.Enqueue(bullet);
            }
        }

        public GameObject GetBullet()
        {
            if (_bulletPool.Count > 0)
            {
                return _bulletPool.Dequeue();
            }
        
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            return bullet;
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
    }
}
