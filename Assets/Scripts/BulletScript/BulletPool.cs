using System.Collections.Generic;
using UnityEngine;

namespace BulletScript
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance;

        [Header("Pool Settings")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int poolSize = 20;

        private Queue<GameObject> bulletPool = new Queue<GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bulletPool.Enqueue(bullet);
            }
        }

        public GameObject GetBullet()
        {
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool.Dequeue();
                bullet.SetActive(true);
                return bullet;
            }
            else
            {
                // Nếu hết đối tượng trong Pool, tạo mới (tùy chọn)
                Debug.LogWarning("Bullet Pool is empty. Expanding pool...");
                return Instantiate(bulletPrefab);
            }
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }
}