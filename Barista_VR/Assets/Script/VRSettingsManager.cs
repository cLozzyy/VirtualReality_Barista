using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class VRSettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle teleportToggle;
    public Toggle snapTurnToggle;
    public Slider vignetteSlider;

    public MonoBehaviour teleportationProvider;
    public MonoBehaviour snapTurnProvider;
    public Volume postProcessingVolume;

    private void Start()
    {
        if (volumeSlider != null) volumeSlider.onValueChanged.AddListener(SetVolume);
        if (teleportToggle != null) teleportToggle.onValueChanged.AddListener(SetTeleport);
        if (snapTurnToggle != null) snapTurnToggle.onValueChanged.AddListener(SetSnapTurn);
        if (vignetteSlider != null) vignetteSlider.onValueChanged.AddListener(SetVignette);

        InitializeSettings();
    }

    private void InitializeSettings()
    {
        if (volumeSlider != null) volumeSlider.value = AudioListener.volume;
        
        if (teleportToggle != null && teleportationProvider != null)
        {
            teleportToggle.isOn = teleportationProvider.enabled;
        }
        
        if (snapTurnToggle != null && snapTurnProvider != null)
        {
            snapTurnToggle.isOn = snapTurnProvider.enabled;
        }
        
        if (vignetteSlider != null && postProcessingVolume != null)
        {
            vignetteSlider.value = postProcessingVolume.weight;
        }
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetTeleport(bool isOn)
    {
        if (teleportationProvider != null)
        {
            teleportationProvider.enabled = isOn;
        }
    }

    public void SetSnapTurn(bool isOn)
    {
        if (snapTurnProvider != null)
        {
            snapTurnProvider.enabled = isOn;
        }
    }

    public void SetVignette(float value)
    {
        if (postProcessingVolume != null)
        {
            postProcessingVolume.weight = value;
        }
    }
}