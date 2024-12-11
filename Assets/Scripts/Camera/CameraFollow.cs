using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0f, 10f, -10f);

    [Header("Smooth Settings")]
    public float smoothSpeed = 0.125f; 

    [Header("Bounds Settings (Optional)")]
    public bool useBounds = false; 
    public Vector2 minBounds; 
    public Vector2 maxBounds; 

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
}