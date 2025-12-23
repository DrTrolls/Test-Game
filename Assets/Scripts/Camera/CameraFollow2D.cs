using UnityEngine;

/// <summary>
/// Simple follow camera with a configurable offset and tilt for a faux top-down perspective.
/// Attach to the Main Camera and assign the player transform.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -8f);
    [SerializeField] private float followSpeed = 6f;
    [SerializeField] private float tiltAngle = 20f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        float lerpFactor = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpFactor);

        Quaternion desiredRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lerpFactor);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
