using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // ← tambah ini

public class ToggleInteractor : MonoBehaviour
{
    [Header("Tangan Kanan")]
    public XRDirectInteractor nearInteractorKanan;
    public XRRayInteractor farInteractorKanan;

    [Header("Tangan Kiri")]
    public XRDirectInteractor nearInteractorKiri;
    public XRRayInteractor farInteractorKiri;

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            bool nearAktif = nearInteractorKanan.enabled;

            nearInteractorKanan.enabled = !nearAktif;
            farInteractorKanan.enabled = nearAktif;
            nearInteractorKiri.enabled = !nearAktif;
            farInteractorKiri.enabled = nearAktif;

            Debug.Log($"Near: {nearInteractorKanan.enabled} | Far: {farInteractorKanan.enabled}");
        }
    }
}