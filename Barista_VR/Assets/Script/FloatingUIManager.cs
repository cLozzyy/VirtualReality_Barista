using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingUIManager : MonoBehaviour
{
    public Transform targetCamera;
    public Transform uiTransform;       
    public float followDistance = 3.5f;
    public float followSpeed = 5f;
    public float heightOffset = 0f;
    public GameObject menuUIPanel;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            menuUIPanel.SetActive(!menuUIPanel.activeSelf);
        }
    }

    private void LateUpdate()
    {
        if (targetCamera == null || uiTransform == null) return;
        if (!menuUIPanel.activeSelf) return;

        Vector3 targetPosition = targetCamera.position + (targetCamera.forward * followDistance);
        targetPosition.y = targetCamera.position.y + heightOffset;

        uiTransform.position = Vector3.Lerp(uiTransform.position, targetPosition, Time.deltaTime * followSpeed);

        Vector3 lookAtPosition = new Vector3(targetCamera.position.x, uiTransform.position.y, targetCamera.position.z);
        uiTransform.LookAt(lookAtPosition);
        uiTransform.Rotate(0, 180, 0);
    }
}