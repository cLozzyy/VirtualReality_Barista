using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("UI Tutorial")]
    public TextMeshPro teksTutorial;

    [Header("Daftar Instruksi")]
    [TextArea]
    public string[] daftarInstruksi;

    [Header("Sistem Petunjuk Visual (Glitter)")]
    public GameObject prefabEfekPetunjuk;
    public Transform[] targetBarangPetunjuk;

    private GameObject efekAktif;
    private Transform targetMengikuti;
    private int stepSekarang = 0;

    // TAMBAHAN: Buat nyimpen status Coroutine biar bisa disetop saat ganti step
    private Coroutine audioLoopCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        if (prefabEfekPetunjuk != null)
        {
            efekAktif = Instantiate(prefabEfekPetunjuk);
            efekAktif.transform.SetParent(null);
        }
        UpdateTeksDanPetunjuk();
    }

    private void Update()
    {
        if (efekAktif != null && efekAktif.activeSelf && targetMengikuti != null)
        {
            efekAktif.transform.position = targetMengikuti.position + new Vector3(0, 0.15f, 0);
        }
    }

    public void LaporSelesai(int stepYangSelesai)
    {
        if (stepSekarang == stepYangSelesai)
        {
            stepSekarang++;
            UpdateTeksDanPetunjuk();
        }
    }

    private void UpdateTeksDanPetunjuk()
    {
        // PENTING: Stop loop suara sebelumnya tiap kali ganti barang/step
        if (audioLoopCoroutine != null)
        {
            StopCoroutine(audioLoopCoroutine);
            audioLoopCoroutine = null;
        }

        if (stepSekarang < daftarInstruksi.Length)
        {
            teksTutorial.text = daftarInstruksi[stepSekarang];

            if (efekAktif != null && stepSekarang < targetBarangPetunjuk.Length)
            {
                targetMengikuti = targetBarangPetunjuk[stepSekarang];

                if (targetMengikuti != null)
                {
                    efekAktif.SetActive(true);

                    ParticleSystem ps = efekAktif.GetComponent<ParticleSystem>();
                    if (ps != null) ps.Play();

                    AudioSource audioEfek = efekAktif.GetComponent<AudioSource>();
                    if (audioEfek != null)
                    {
                        // Mulai looping 3 detik dan simpan di variabel audioLoopCoroutine
                        audioLoopCoroutine = StartCoroutine(LoopManualTigaDetik(audioEfek, 3.0f));
                    }
                }
                else
                {
                    efekAktif.SetActive(false);
                }
            }
        }
        else
        {
            teksTutorial.text = "Bagus! Kamu sudah siap melayani pelanggan sungguhan!";
            if (efekAktif != null) efekAktif.SetActive(false);
        }
    }

    // FUNGSI BARU: Looping manual khusus 3 detik pertama
    private IEnumerator LoopManualTigaDetik(AudioSource audio, float durasiLoop)
    {
        // Cek terus selama efek debu ini statusnya aktif di game
        while (efekAktif != null && efekAktif.activeInHierarchy)
        {
            audio.time = 0f; // Paksa balikin suara ke detik 0
            audio.Play();    // Mainkan suaranya

            // Tunggu 3 detik sebelum kode muter balik ke atas (ngulang ke detik 0 lagi)
            yield return new WaitForSeconds(durasiLoop);
        }
    }
}