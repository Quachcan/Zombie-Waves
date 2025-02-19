
using System.Collections.Generic;
using Game.Scripts.BulletScript;
using Game.Scripts.EnemyScripts;
using Game.Scripts.EnemyScripts.RegularEnemy;
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
        public float fireRate;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float bulletLifeTime;
        [SerializeField]
        private float nextFireTime;
        [SerializeField] 
        public int bulletDamage;
        [SerializeField] 
        private float attackRange;
        [SerializeField]
        private List<Enemy> enemies = new List<Enemy>();
        //private List<Enemy> enemiesInRange = new List<Enemy>();
        private float targetUpdateInterval = 0.2f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Initialize()
        {
            InvokeRepeating(nameof(UpdateTargetEnemy), 0f, targetUpdateInterval);
        }
        
        void Update()
        {
            if (nextFireTime > 0)
            {
                nextFireTime -= Time.deltaTime;
            }
            
            if (targetEnemy != null)
            {
                RotatePlayerTowardsTarget();
                if (Time.time > nextFireTime)
                {
                    if(CanShoot())
                    {
                        Shoot();
                        nextFireTime = Time.time + fireRate;
                    }
                    else
                    {
                        // Debug log để kiểm tra góc
                        float angle = Vector3.Angle(firePoint.forward, (GetEnemyTargetPoint(targetEnemy) - firePoint.position).normalized);
                        Debug.Log("Không bắn vì góc = " + angle);
                    }
                }
                
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

        private Vector3 GetEnemyTargetPoint(Enemy enemy)
        {
            if (enemy != null && enemy.hitPoint != null)
            {
                return enemy.hitPoint.position;
            }
            return enemy != null ? enemy.transform.position : Vector3.zero;
        }


        private void UpdateTargetEnemy()
        {
            targetEnemy = GetClosestEnemy();
            if (targetEnemy != null)
            {
                Debug.Log("Target enemy updated: " + targetEnemy.name);
            }
        }

        private void RotatePlayerTowardsTarget()
        {
            Vector3 targetPoint = GetEnemyTargetPoint(targetEnemy);
            Vector3 direction = (targetPoint - firePoint.position).normalized;
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
            Vector3 direction = (GetEnemyTargetPoint(targetEnemy) - transform.position).normalized;
            direction.y = 0;
            return direction;
        }

        private void Shoot()
        {
            if (!CanShoot()) return;

            GameObject bullet = SpawnBullet();
            if (bullet != null)
            {
                InitializeBullets(bullet);
            }
        }

        private bool CanShoot()
        {
            if (targetEnemy == null) return false;
            
            Vector3 directionToTarget = (GetEnemyTargetPoint(targetEnemy) - firePoint.position).normalized;
            float angleToTarget = Vector3.Angle(firePoint.forward, directionToTarget);
            
            return angleToTarget <= 15f;
        }

        private GameObject SpawnBullet()
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                Vector3 targetPoint = GetEnemyTargetPoint(targetEnemy);
                bullet.transform.rotation = Quaternion.LookRotation(targetPoint - firePoint.position);
                bullet.SetActive(true);
            }
            return bullet;
        }

        private void InitializeBullets(GameObject bullet)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                Vector3 targetPoint = GetEnemyTargetPoint(targetEnemy);
                Vector3 shootDirection = (targetPoint - firePoint.position).normalized;
                bulletScript.Initialize(shootDirection, bulletDamage);
            }
        }
        
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

        public void IncreaseDamage(int amount)
        {
            bulletDamage += amount;
            Debug.Log("Increased damage: " + amount);
        }

        public void IncreaseFireRate(int amount)
        {
            fireRate -= amount;
            fireRate = Mathf.Max(0.05f, fireRate);
            Debug.Log("Increased fire rate: " + fireRate);
        }
    }
}
