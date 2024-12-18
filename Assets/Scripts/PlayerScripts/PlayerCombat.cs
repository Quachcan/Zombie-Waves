
using System.Collections.Generic;
using BulletScript;
using EnemyScripts;
using UnityEngine;


namespace PlayerScripts

{
    public class PlayerCombat : MonoBehaviour
    {
        public static PlayerCombat Instance;
        [Header("References")]
        [SerializeField]
        private Enemy targetEnemy;
        [SerializeField]
        private BulletPool bulletPool;
        
        [Header("Combat Settings")]
        [SerializeField]
        private Transform firePoint;
        [SerializeField]
        private float fireRate;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float bulletLifeTime;
        [SerializeField]
        private float fireCooldown;
        [SerializeField] 
        private int bulletDamage;
        [SerializeField] 
        private float attackRange;
        [SerializeField]
        private List<Enemy> enemiesInRange = new List<Enemy>();
        private float _targetUpdateInterval = 0.2f;
        private List<Enemy> enemies = new List<Enemy>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            InvokeRepeating(nameof(UpdateTargetEnemy), 0f, _targetUpdateInterval);
        }
        
        void Update()
        {
            if (fireCooldown > 0)
            {
                fireCooldown -= Time.deltaTime;
            }
            
            if (targetEnemy != null)
            {
                RotatePlayerTowardsTarget();
            }

            if (fireCooldown <= 0 && targetEnemy != null)
            {
                Shoot();
            }
        }

        private Enemy GetClosestEnemy()
        {
            Enemy closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= attackRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        private void UpdateTargetEnemy()
        {
            targetEnemy = GetClosestEnemy();
        }

        private void RotatePlayerTowardsTarget()
        {
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            direction.y = 0;
            
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        public bool HasTarget()
        {
            if (targetEnemy == null)
            {
                return false;
            }
            if (!targetEnemy.gameObject.activeInHierarchy)
            {
                return false;
            }
            return true;
        }

        public Vector3 GetTargetDirection()
        {
            if (targetEnemy == null) return Vector3.zero;
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            direction.y = 0;
            return direction;
        }

        private void Shoot()
        {
            if (!CanShoot()) return;
            RotatePlayerTowardsTarget();

            GameObject bullet = SpawnBullet();
            if (bullet != null)
            {
                InitializeBullets(bullet);
            }
            fireCooldown = fireRate;
        }

        private bool CanShoot()
        {
            if (targetEnemy == null) return false;
            
            Vector3 directionToTarget = (targetEnemy.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            
            return angleToTarget <= 5f;
        }

        private GameObject SpawnBullet()
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.LookRotation(targetEnemy.transform.position - firePoint.forward);
                bullet.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to spawn bullet from pool.");
            }
            return bullet;
        }

        private void InitializeBullets(GameObject bullet)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                Vector3 shootDirection = (targetEnemy.transform.position - firePoint.position).normalized;
                bulletScript.Initialize(shootDirection, bulletDamage);
            }
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Enemy"))
        //     {
        //         Enemy enemy = other.GetComponent<Enemy>();
        //         if (enemy != null && !enemiesInRange.Contains(enemy))
        //         {
        //             enemiesInRange.Add(enemy);
        //         }
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.CompareTag("Enemy"))
        //     {
        //         Enemy enemy = other.GetComponent<Enemy>();
        //         if (enemy != null && enemiesInRange.Contains(enemy))
        //         {
        //             enemiesInRange.Remove(enemy);
        //         }
        //     }
        // }
        
        public void RegisterEnemy(Enemy enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
        
        public void UnregisterEnemy(Enemy enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }
    }
}
