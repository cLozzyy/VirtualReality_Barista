using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KembaliKeAwal : MonoBehaviour
{
    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;
    private Rigidbody rb;

    void Start() { posisiAwal = transform.position; rotasiAwal = transform.rotation; rb = GetComponent<Rigidbody>(); }

    public void ResetBarang()
    {
        // 1. Force Eject dari Socket (Casting terbaru)
        var socket = GetComponentInParent<XRSocketInteractor>();
        if (socket != null)
        {
            socket.interactionManager.SelectExit((IXRSelectInteractor)socket, (IXRSelectInteractable)GetComponent<XRGrabInteractable>());
        }

        // 2. Reset Posisi & Fisika
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;
        if (rb != null) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        // 3. Reset Isi
        GelasKopi gelas = GetComponent<GelasKopi>();
        if (gelas != null) gelas.KosongkanGelas();
        PortafilterKopi pf = GetComponent<PortafilterKopi>();
        if (pf != null) pf.ResetPortafilter();
    }
}