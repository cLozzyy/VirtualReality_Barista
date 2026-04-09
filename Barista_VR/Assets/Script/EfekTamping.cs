using UnityEngine;
using System.Collections;

public class EfekTamping : MonoBehaviour
{
    [Header("Objek yang Dimodifikasi")]
    public Transform gundukanKopi;         // Masukkan objek 'Porta filter_Coffee' ke sini
    public Transform attachTransformSocket; // Masukkan objek 'TitikTempel' ke sini

    [Header("Pengaturan Animasi")]
    public float jarakTekan = 0.02f;   // Seberapa dalam tampernya turun ke bawah
    public float batasRataKopi = 0.2f; // Skala Y kopi setelah dipipihkan (makin kecil makin tipis)
    public float durasiAnimasi = 0.6f; // Kecepatan gerakan menekan

    // Fungsi ini akan dipanggil oleh Socket saat Tamper masuk
    public void MulaiTamping()
    {
        // Mengecek apakah kopinya sedang menyala (sudah diisi)
        if (gundukanKopi != null && gundukanKopi.gameObject.activeSelf)
        {
            StartCoroutine(ProsesAnimasi());
        }
    }

    private IEnumerator ProsesAnimasi()
    {
        // Menyimpan data posisi dan ukuran awal
        Vector3 skalaKopiAwal = gundukanKopi.localScale;
        Vector3 skalaKopiTarget = new Vector3(skalaKopiAwal.x, batasRataKopi, skalaKopiAwal.z);

        Vector3 posisiAttachAwal = attachTransformSocket.localPosition;
        Vector3 posisiAttachTarget = posisiAttachAwal - new Vector3(0, jarakTekan, 0);

        float waktu = 0;

        // Proses animasi perlahan
        while (waktu < durasiAnimasi)
        {
            waktu += Time.deltaTime;
            float persentase = waktu / durasiAnimasi;

            // Memipihkan kopi
            gundukanKopi.localScale = Vector3.Lerp(skalaKopiAwal, skalaKopiTarget, persentase);

            // Menurunkan tamper (dengan menurunkan titik socketnya)
            attachTransformSocket.localPosition = Vector3.Lerp(posisiAttachAwal, posisiAttachTarget, persentase);

            yield return null;
        }
        TutorialManager.instance.LaporSelesai(3);
        Debug.Log("Tamping selesai! Kopi sudah padat.");
    }
}