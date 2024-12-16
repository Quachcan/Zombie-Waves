
using System.Collections.Generic;
using BulletScript;
using EnemyScripts;
using UnityEngine;


namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
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
        
        
        private List<Enemy> enemiesInRange = new List<Enemy>();
        
        private void Start()
        {
            SphereCollider attackRangeCollider = gameObject.AddComponent<SphereCollider>();
            attackRangeCollider.isTrigger = true;
            attackRangeCollider.radius = attackRange;
            bulletPool = BulletPool.Instance;
        }

        // Update is called once per frame
        void Update()
        {

            enemiesInRange.RemoveAll(enemy => enemy == null);
            if (fireCooldown > 0)
            {
                fireCooldown -= Time.deltaTime;
            }

            targetEnemy = GetClosestEnemy();
            
            if (targetEnemy != null)
            {
                RotatePlayerTowardsTarget();
            }

            if (fireCooldown <= 0 && targetEnemy != null)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }

        private Enemy GetClosestEnemy()
        {
            Enemy closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (Enemy enemy in enemiesInRange)
            {
                if(enemy == null) continue;
                
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
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
            Enemy closetEnemy = GetClosestEnemy();
            
            if (closetEnemy ==null) return;
            
            Vector3 directionToTarget = (targetEnemy.transform.position - transform.position).normalized;
            directionToTarget.y = 0;
            
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget > 5f)
            {
                return;
            }
            
            RotatePlayerTowardsTarget();
            
            GameObject bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(closetEnemy.transform.position - firePoint.position);
            bullet.SetActive(true);
            
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(targetEnemy.transform, bulletDamage);
                StartCoroutine(bulletScript.BulletCoroutineLifeTime(bulletLifeTime));
            }
            
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (closetEnemy.transform.position - firePoint.position).normalized * bulletSpeed;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null && !enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null && enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Remove(enemy);
                }
            }
        }

        public void RemoveEnemy(Enemy enemy)
        {
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}
