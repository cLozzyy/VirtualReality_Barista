using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FloatingUIManager : MonoBehaviour
{
    public Transform targetCamera;
    public Transform uiTransform;
    public float followDistance = 3.5f;
    public float followSpeed = 5f;
    public float heightOffset = 0f;
    public GameObject menuUIPanel;

    public Slider volumeSlider;
    public Slider vignetteSlider;
    public Toggle teleportToggle;
    public Toggle snapTurnToggle;

    public Behaviour teleportationProvider;
    public Behaviour snapTurnProvider;

    public Volume globalVolume;
    private Vignette vignetteProfile;

    private void Start()
    {
        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out vignetteProfile);
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            OnVolumeChanged(volumeSlider.value);
        }
        
        if (vignetteSlider != null)
        {
            vignetteSlider.onValueChanged.AddListener(OnVignetteChanged);
            OnVignetteChanged(vignetteSlider.value);
        }

        if (teleportToggle != null)
        {
            teleportToggle.onValueChanged.AddListener(OnTeleportToggled);
            if (teleportationProvider != null) teleportationProvider.enabled = teleportToggle.isOn;
        }

        if (snapTurnToggle != null)
        {
            snapTurnToggle.onValueChanged.AddListener(OnSnapTurnToggled);
            if (snapTurnProvider != null) snapTurnProvider.enabled = snapTurnToggle.isOn;
        }
    }

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

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    private void OnVignetteChanged(float value)
    {
        if (vignetteProfile != null)
        {
            vignetteProfile.intensity.value = value;
        }
    }

    private void OnTeleportToggled(bool isOn)
    {
        if (teleportationProvider != null)
        {
            teleportationProvider.enabled = isOn;
        }
    }

    private void OnSnapTurnToggled(bool isOn)
    {
        if (snapTurnProvider != null)
        {
            snapTurnProvider.enabled = isOn;
        }
    }
}