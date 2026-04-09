using UnityEngine;

public class SistemPelanggan : MonoBehaviour
{
    [Header("Pergerakan")]
    public Transform titikKasir; // Titik tujuan (depan meja)
    public float kecepatan = 1.5f;

    [Header("UI Resep")]
    public GameObject canvasResep; // Papan resep yang mau dimunculkan

    private bool sudahDiKasir = false;

    void Update()
    {
        // Kalau pelanggan belum sampai di kasir, dia akan terus jalan
        if (!sudahDiKasir && titikKasir != null)
        {
            // Bergerak perlahan menuju titik target
            transform.position = Vector3.MoveTowards(transform.position, titikKasir.position, kecepatan * Time.deltaTime);

            // Mengecek apakah jarak pelanggan sudah dekat banget sama kasir
            if (Vector3.Distance(transform.position, titikKasir.position) < 0.1f)
            {
                sudahDiKasir = true;
                Debug.Log("Pelanggan sudah menunggu pesanan.");
            }
        }
    }

    // Fungsi ini akan dipanggil saat kita nembak laser/menyentuh pelanggan
    public void TampilkanResep()
    {
        // Resep cuma bisa dilihat kalau pelanggan udah sampai di kasir
        if (sudahDiKasir && canvasResep != null)
        {
            canvasResep.SetActive(true);
            Debug.Log("Resep ditampilkan!");
            TutorialManager.instance.LaporSelesai(0);
        }
    }
}