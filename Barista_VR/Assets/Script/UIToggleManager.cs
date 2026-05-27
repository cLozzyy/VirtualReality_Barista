using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggleManager : MonoBehaviour
{
    public GameObject uiPanel;
    public InputAction toggleAction;

    private void OnEnable()
    {
        toggleAction.Enable();
        toggleAction.performed += ToggleVisibility;
    }

    private void OnDisable()
    {
        toggleAction.Disable();
        toggleAction.performed -= ToggleVisibility;
    }

    private void ToggleVisibility(InputAction.CallbackContext context)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    }
}   