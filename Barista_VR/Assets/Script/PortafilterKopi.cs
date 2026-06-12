using UnityEngine;
using TMPro;

public class PortafilterKopi : MonoBehaviour
{
    [Header("Visual Kopi Portafilter")]
    public GameObject portaFilter_Coffee;

    [Header("Layar Digital")]
    public TextMeshPro teksTimbangan;

    [Header("Data Fisik Kopi")]
    public float totalBeratKopi = 0f;
    public bool sudahAdaKopi = false;

    private void Start()
    {
        sudahAdaKopi = false;
        if (portaFilter_Coffee != null) portaFilter_Coffee.SetActive(false);

        // Di awal game, karena dia nempel di timbangan, set tulisan awal (0.0 g)
        UpdateLayarTimbangan();
    }

    public void ResetPortafilter()
    {
        sudahAdaKopi = false;
        totalBeratKopi = 0f;
        // Panggil fungsi untuk matikan visual kopi di portafilter
        if (portaFilter_Coffee != null) portaFilter_Coffee.SetActive(false);

        // Update timbangan kalau perlu
        UpdateLayarTimbangan();
        Debug.Log("Portafilter berhasil di-reset.");
    }

    // --- FUNGSI UTAMA SAAT MENERIMA KOPI ---
    public void TerimaKopi()
    {
        if (portaFilter_Coffee != null)
        {
            portaFilter_Coffee.SetActive(true); // Gundukan kopi menyala
            totalBeratKopi = 15.0f;             // Set langsung ke 15 gram biar presisi
            sudahAdaKopi = true;

            // Update layar timbangan digital biar langsung berubah jadi 15.0 g
            UpdateLayarTimbangan();

            // SINKRONISASI TUTURAL DINAMIS:
            // Ambil index step aktif di TV saat ini secara otomatis (bukan angka kaku lagi)
            if (TutorialManager.instance != null)
            {
                // Kita lapor kalau langkah "Ambil sendok & scoop" sudah beres
                TutorialManager.instance.InteractObjek();
            }

            Debug.Log("Sukses! Kopi dituang.");
        }
    }

    // --- DETEKSI FISIK (TRIGGER) ---

    private void OnTriggerEnter(Collider other)
    {
        // A. Jika menyentuh Timbangan -> Tampilkan berat saat ini
        if (other.CompareTag("Timbangan"))
        {
            UpdateLayarTimbangan();
            Debug.Log("Portafilter ditaruh di timbangan. Menampilkan berat sekarang.");
        }

        // B. FIX TANPA GANTI TAG: Cek apakah nama objeknya mengandung kata "Sendok" atau "Spoon"
        // Cara ini aman karena Tag sendok lu mau tetep "Untagged" atau apa pun, kodenya tetep jalan!
        if ((other.name.Contains("Sendok") || other.name.Contains("Spoon")) && !sudahAdaKopi)
        {
            // PENGAMAN TAMBAHAN: Pastikan TV tutorial lu emang lagi di Step Kopi (bukan pas player lagi disuruh nyendok gula)
            if (TutorialManager.instance != null)
            {
                int stepSekarang = TutorialManager.instance.AmbilStepSekarang();

                // Berdasarkan urutan, step ke-2 adalah menuang kopi ke portafilter
                if (stepSekarang == 2)
                {
                    Debug.Log("[Portafilter] Sendok terdeteksi pada Step Kopi! Memproses tuang bubuk...");
                    TerimaKopi();
                }
                else
                {
                    Debug.LogWarning("[Portafilter] Sendok menyentuh portafilter, tapi ditolak karena TV lagi gak nyuruh nuang kopi!");
                }
            }
        }
    }

    // Pas portafilter DIANGKAT/DICOPOT dari timbangan -> Angka layar jadi 0.0 g
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Timbangan"))
        {
            if (teksTimbangan != null)
            {
                teksTimbangan.text = "0.0 g";
            }
            Debug.Log("Portafilter diangkat dari timbangan. Layar kembali 0.");
        }
    }

    // Fungsi pembantu biar gak nulis kode ToString berulang-ulang
    private void UpdateLayarTimbangan()
    {
        if (teksTimbangan != null)
        {
            teksTimbangan.text = totalBeratKopi.ToString("F1") + " g";
        }
    }
}