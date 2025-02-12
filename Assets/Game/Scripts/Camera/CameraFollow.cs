using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Managers;
using Managers;
using UnityEngine;

namespace Game.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance;
    
        [Header("Target Settings")]
        public Transform target;

        [Header("Camera Offset")]
        public Vector3 offset = new Vector3(0f, 10f, -10f);

        [Header("Smooth Settings")]
        public float smoothSpeed = 0.125f; 

        [Header("Bounds Settings (Optional)")]
        public bool useBounds = true; 
        public Vector2 minBounds; 
        public Vector2 maxBounds;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (useBounds)
            {
                InitializeBounds();
            }
        }
        
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }
        
            Vector3 desiredPosition = target.position + offset;
        
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
            if (useBounds)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
                smoothedPosition.z = Mathf.Clamp(smoothedPosition.z, minBounds.y, maxBounds.y);
            }
        
            transform.position = smoothedPosition;
        
            transform.LookAt(target);
            
        }

        private void InitializeBounds()
        {
            Bounds mapBounds = GameManager.Instance.GetBounds();
            
            minBounds = new Vector2(mapBounds.center.x - mapBounds.extents.x, mapBounds.center.z - mapBounds.extents.z);
            maxBounds = new Vector2(mapBounds.center.x + mapBounds.extents.x, mapBounds.center.z + mapBounds.extents.z);

            
            // minBounds = new Vector2(mapBounds.min.x, maxBounds.min.z);
            // maxBounds = new Vector2(mapBounds.max.x, maxBounds.max.z);
            
            //Debug.Log($"✅ Camera Bounds Set: Min({minBounds}), Max({maxBounds})");
        }
    }
}