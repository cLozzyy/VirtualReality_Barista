using UnityEngine;

public class KembaliKeAwal : MonoBehaviour
{
    [Header("Pengaturan Jatuh")]
    [Tooltip("Jika posisi Y barang lebih rendah dari angka ini, barang akan reset.")]
    public float batasJatuhY = 0.2f;

    // Variabel untuk menyimpan data awal
    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;
    private Rigidbody rb;

    void Start()
    {
        // 1. Rekam posisi dan rotasi awal saat game baru di-play
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;

        // Ambil komponen Rigidbody (fisika) dari barang ini
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 2. Cek setiap saat, apakah barang jatuh melewati batas lantai?
        if (transform.position.y < batasJatuhY)
        {
            ResetBarang();
        }
    }

    public void ResetBarang()
    {
        // 3. Kembalikan ke tempat awal
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;

        // 4. PENTING: Matikan kecepatan jatuhnya!
        // Kalau nggak, pas balik ke meja dia bakal punya gaya gravitasi sisa dan jatuh lagi
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;        // Stop pergerakan linear
            rb.angularVelocity = Vector3.zero; // Stop putaran/gulingan
        }

        Debug.Log(gameObject.name + " jatuh dan dikembalikan ke meja!");
    }
}