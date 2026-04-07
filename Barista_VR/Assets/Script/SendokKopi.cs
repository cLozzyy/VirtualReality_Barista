using UnityEngine;

public class SendokKopi : MonoBehaviour
{
    [Header("Visual Kopi di Sendok")]
    public GameObject gundukanKopi;
    public bool isIsiKopi = false;

    // Fungsi ini dipanggil otomatis saat sendok menyentuh area Trigger apapun
    private void OnTriggerEnter(Collider other)
    {
        // 1. JIKA NYENTUH TOPLES & SENDOK KOSONG -> Ambil Kopi
        if (other.CompareTag("ToplesKopi") && !isIsiKopi)
        {
            AmbilKopi();
        }
        // 2. JIKA NYENTUH PORTAFILTER & SENDOK ADA ISINYA -> Tuang Kopi
        else if (other.CompareTag("Portafilter") && isIsiKopi)
        {
            // Langsung tuang tanpa perlu dimiringkan
            TuangKopi(other.gameObject);
        }
    }

    private void AmbilKopi()
    {
        gundukanKopi.SetActive(true);
        isIsiKopi = true;
        Debug.Log("Berhasil ambil kopi dari toples!");
    }

    private void TuangKopi(GameObject areaTuang)
    {
        gundukanKopi.SetActive(false);
        isIsiKopi = false;

        PortafilterKopi pf = areaTuang.GetComponent<PortafilterKopi>();

        if (pf != null)
        {
            pf.TerimaKopi();
        }
        else
        {
            Debug.LogWarning("Script PortafilterKopi tidak ditemukan di objek: " + areaTuang.name);
        }
    }
}