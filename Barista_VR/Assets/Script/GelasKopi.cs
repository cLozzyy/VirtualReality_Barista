using UnityEngine;
using System.Collections;

public class GelasKopi : MonoBehaviour
{
    [Header("Visual Air Kopi")]
    public Transform pivotAirKopi; // Masukkan objek 'PivotAir' ke sini

    // VARIABEL BARU: Untuk memegang komponen 'mata' si air
    private MeshRenderer rendererAir;

    [Header("Pengaturan Pengisian")]
    public float targetPenuhY = 1f; // Target tinggi scale Y saat penuh
    public float durasiIsi = 3f;    // Waktu pengisian (detik)

    private bool sedangIsi = false; // Mencegah isi ulang pas lagi proses
    private bool sudahPenuh = false;

    void Start()
    {
        // 1. Ambil komponen MeshRenderer dari objek 'VisualKopiHitam'
        // Kita cari komponen MeshRenderer yang ada di anak (child) objek 'pivotAirKopi'
        rendererAir = pivotAirKopi.GetComponentInChildren<MeshRenderer>();

        if (rendererAir != null)
        {
            // 2. Matikan 'mata'-nya di awal game. Air jadi gaib.
            rendererAir.enabled = false;
        }
        else
        {
            Debug.LogError("Gagal menemukan MeshRenderer di anak objek PivotAir!");
        }

        // 3. Kempeskan airnya (Scale Y jadi 0)
        Vector3 skalaAwal = pivotAirKopi.localScale;
        skalaAwal.y = 0f;
        pivotAirKopi.localScale = skalaAwal;
    }

    public void MulaiIsiAir()
    {
        // Cek dulu, jangan sampe gelas yang udah penuh diisi lagi
        if (!sedangIsi && !sudahPenuh)
        {
            StartCoroutine(ProsesIsi());
        }
    }

    private IEnumerator ProsesIsi()
    {
        sedangIsi = true;
        Debug.Log("Mesin menyala! Mengisi kopi ke gelas...");

        // 4. NYALAKAN KEMBALI 'mata'-nya sebelum animasi mulai.
        // Air jadi kelihatan lagi, tapi ukurannya masih 0.
        if (rendererAir != null)
        {
            rendererAir.enabled = true;
        }

        float waktu = 0;
        Vector3 skalaTarget = new Vector3(pivotAirKopi.localScale.x, targetPenuhY, pivotAirKopi.localScale.z);

        while (waktu < durasiIsi)
        {
            waktu += Time.deltaTime;
            float persentase = waktu / durasiIsi;

            // Animasi scale naik dari 0 ke targetPenuhY
            pivotAirKopi.localScale = Vector3.Lerp(new Vector3(skalaTarget.x, 0f, skalaTarget.z), skalaTarget, persentase);
            yield return null;
        }

        sudahPenuh = true;
        sedangIsi = false;
        Debug.Log("Gelas Penuh!");

        // Panggil Tutorial Manager kalau kamu pakai!
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.LaporSelesai(5);
        }
    }
}