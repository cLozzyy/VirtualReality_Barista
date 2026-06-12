using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KembaliKeAwal : MonoBehaviour
{
    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ResetBarang()
    {
        // Jalankan proses reset dengan jeda waktu sekejap
        StartCoroutine(ProsesResetAman());
    }

    private IEnumerator ProsesResetAman()
    {
        // 1. Matikan radar interaksi! Ini bikin socket langsung "lupa" sama portafilter
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // Lepaskan dari objek parent (seperti mesin espresso)
        transform.SetParent(null);

        // 2. Bekukan fisika sementara biar nggak mental pas di-teleport
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 3. Teleport ke meja (Posisi Awal)
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;

        // 4. Reset Isi Kopi / Gelas
        GelasKopi gelas = GetComponent<GelasKopi>();
        if (gelas != null) gelas.KosongkanGelas();

        PortafilterKopi pf = GetComponent<PortafilterKopi>();
        if (pf != null) pf.ResetPortafilter();

        // KUNCI UTAMA: Tunggu 1 frame sampai Unity merender posisi baru ini di sistem fisika
        yield return new WaitForEndOfFrame();

        // 5. Nyalakan lagi interaksi dan fisikanya setelah aman di atas meja
        if (rb != null) rb.isKinematic = false;
        if (grabInteractable != null) grabInteractable.enabled = true;
    }
}