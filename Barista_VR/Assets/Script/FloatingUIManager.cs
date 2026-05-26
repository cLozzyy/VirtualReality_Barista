using UnityEngine;

public class FloatingUIManager : MonoBehaviour
{
    public Transform targetCamera;
    public float followDistance = 10f;
    public float followSpeed = 5f;
    public float heightOffset = -5f;

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 targetPosition = targetCamera.position + (targetCamera.forward * followDistance);
        targetPosition.y = targetCamera.position.y + heightOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        Vector3 lookAtPosition = new Vector3(targetCamera.position.x, transform.position.y, targetCamera.position.z);
        transform.LookAt(lookAtPosition);
        transform.Rotate(0, 180, 0);
    }
}
