using UnityEngine;
using TMPro;

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
    private Transform targetMengikuti; // Variabel baru untuk nyimpen target saat ini
    private int stepSekarang = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        if (prefabEfekPetunjuk != null)
        {
            efekAktif = Instantiate(prefabEfekPetunjuk);
            // PENTING: Jangan jadikan child siapa-siapa. Biarkan bebas di luar.
            efekAktif.transform.SetParent(null);
        }
        UpdateTeksDanPetunjuk();
    }

    private void Update()
    {
        // Setiap frame (saat game jalan), paksa partikel pindah ke atas barang target
        if (efekAktif != null && efekAktif.activeSelf && targetMengikuti != null)
        {
            // Mengikuti posisi barang + naik 15 cm (0.15f) ke atas
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
        if (stepSekarang < daftarInstruksi.Length)
        {
            teksTutorial.text = daftarInstruksi[stepSekarang];

            if (efekAktif != null && stepSekarang < targetBarangPetunjuk.Length)
            {
                // Set barang mana yang harus dibuntuti sekarang
                targetMengikuti = targetBarangPetunjuk[stepSekarang];

                if (targetMengikuti != null)
                {
                    efekAktif.SetActive(true);

                    // Mainkan ulang partikelnya biar seger
                    ParticleSystem ps = efekAktif.GetComponent<ParticleSystem>();
                    if (ps != null) ps.Play();
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
}