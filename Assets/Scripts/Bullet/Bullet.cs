using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Transform target;

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
