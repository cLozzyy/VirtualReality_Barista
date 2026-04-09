using System.Collections;
using UnityEngine;
 // Wajib untuk akses komponen XR

// Pastikan komponen XRSocketInteractor ada di objek ini
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class EfekTamping : MonoBehaviour
{
    [Header("Objek yang Dimodifikasi")]
    [Tooltip("Tarik objek gundukan kopi portafilter 'Porta filter_Coffee' ke sini.")]
    public Transform gundukanKopi;

    [Tooltip("Tarik objek 'TitikTempel' (anak objek dari socket) ke sini.")]
    public Transform attachTransformSocket;

    [Header("Pengaturan Animasi Tamping")]
    [Tooltip("Seberapa dalam tamper akan menekan ke bawah (dalam meter).")]
    public float jarakTekan = 0.02f;

    [Tooltip("Skala Y kopi setelah dipipihkan (makin kecil makin tipis).")]
    public float batasRataKopi = 0.2f;

    [Tooltip("Durasi animasi menekan (dalam detik).")]
    public float durasiAnimasi = 0.6f;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketTamping; // Untuk memegang komponen socket
    private Vector3 posisiAttachAwal;          // Menyimpan posisi attach awal socket

    void Awake()
    {
        // Ambil komponen socket di objek ini
        socketTamping = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    void Start()
    {
        // Simpan posisi attach awal saat game mulai
        if (attachTransformSocket != null)
        {
            posisiAttachAwal = attachTransformSocket.localPosition;
        }
        else
        {
            Debug.LogError("Gagal menemukan AttachTransformSocket! Tarik objek 'TitikTempel' ke Inspector.");
        }
    }

    // Fungsi ini akan dipanggil oleh Socket Event 'Select Entered' saat Tamper masuk
    public void MulaiTamping()
    {
        // Mengecek apakah kopinya sedang menyala (sudah diisi) dan socket tidak kosong
        if (gundukanKopi != null && gundukanKopi.gameObject.activeSelf && socketTamping.hasSelection)
        {
            StartCoroutine(ProsesAnimasi());
        }
    }

    private IEnumerator ProsesAnimasi()
    {
        Debug.Log("Proses Tamping Dimulai...");

        // 1. Simpan data skala awal kopi
        Vector3 skalaKopiAwal = gundukanKopi.localScale;
        Vector3 skalaKopiTarget = new Vector3(skalaKopiAwal.x, batasRataKopi, skalaKopiAwal.z);

        // 2. Hitung posisi target attach socket (posisi awal - jarak tekan ke bawah)
        Vector3 posisiAttachTarget = posisiAttachAwal - new Vector3(0, jarakTekan, 0);

        float waktu = 0;

        // 3. Proses animasi perlahan menggunakan LERP
        while (waktu < durasiAnimasi)
        {
            waktu += Time.deltaTime;
            float persentase = waktu / durasiAnimasi;

            // Memipihkan skala kopi secara perlahan
            gundukanKopi.localScale = Vector3.Lerp(skalaKopiAwal, skalaKopiTarget, persentase);

            // Menurunkan tamper secara perlahan (dengan menurunkan titik attach socket)
            attachTransformSocket.localPosition = Vector3.Lerp(posisiAttachAwal, posisiAttachTarget, persentase);

            yield return null; // Tunggu satu frame
        }

        Debug.Log("Animasi Selesai, Reset Tamper...");

        // 4. Proses Reset Tamper Otomatis

        // Ambil objek tamper yang sedang menempel di socket
        UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable tamperInteractable = socketTamping.firstInteractableSelected;
        if (tamperInteractable != null)
        {
            GameObject objekTamper = tamperInteractable.transform.gameObject;

            // Matikan socket sebentar agar tamper terlepas
            socketTamping.enabled = false;

            // Panggil fungsi ResetBarang dari script KembaliKeAwal milik si Tamper
            // Pastikan tamper punya script KembaliKeAwal.cs ya!
            KembaliKeAwal scriptReset = objekTamper.GetComponent<KembaliKeAwal>();
            if (scriptReset != null)
            {
                scriptReset.ResetBarang();
            }
            else
            {
                Debug.LogWarning("Gagal reset: Objek Tamper tidak punya script KembaliKeAwal.cs!");
            }

            // Kembalikan posisi attach socket ke posisi awal agar socket siap dipakai lagi
            attachTransformSocket.localPosition = posisiAttachAwal;

            // Nyalakan kembali socket setelah tamper pergi
            Invoke("NyalakanSocketLagi", 0.5f);
        }

        Debug.Log("Tamping Selesai! Kopi sudah padat dan Tamper kembali ke meja.");

        // (Opsional) Lapor ke Tutorial Manager
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.LaporSelesai(3);
        }
    }

    private void NyalakanSocketLagi()
    {
        socketTamping.enabled = true;
    }
}