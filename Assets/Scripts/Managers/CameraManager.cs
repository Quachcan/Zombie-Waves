using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform;
    private Vector3 _cameraFollowVelocity = Vector3.zero;
    
    public float cameraFollowSpeed = 0.2f;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    private void FollowTarget()
    {
        Vector3 targetPosition  = Vector3.SmoothDamp(transform.position , targetTransform.position , ref _cameraFollowVelocity , cameraFollowSpeed);
        
        transform.position = targetPosition;
    }
}
