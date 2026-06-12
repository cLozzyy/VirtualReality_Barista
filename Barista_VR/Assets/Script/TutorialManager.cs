using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public void TampilkanSelesai(string pesan)
    {
        if (teksTutorial != null)
        {
            teksTutorial.text = pesan;
        }

        // Matikan glitter kalau ada
        UpdateGlitterVisual(false);
        Debug.Log("Game Tamat: " + pesan);
    }

    public void ResetTutorial()
    {
        stepSekarang = 0;
        PerbaruiTampilanTutorial();
        Debug.Log("[TutorialManager] Tutorial di-reset untuk pelanggan baru.");
    }

    [Header("UI Tutorial 3D (Monitor TV Dunia Game)")]
    public TextMeshPro teksTutorial;

    [Header("Sistem Petunjuk Visual (Glitter)")]
    public GameObject prefabEfekPetunjuk;

    private List<string> instruksiAktif = new List<string>();
    private List<Transform> rutePetunjukAktif = new List<Transform>();

    private GameObject efekAktif;
    private Transform targetMengikuti;
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
            efekAktif.transform.SetParent(null);
            efekAktif.SetActive(false);
        }

        if (teksTutorial != null)
        {
            teksTutorial.text = "Arahkan laser ke pelanggan dan tekan tombol untuk melayani.";
        }
    }

    private void Update()
    {
        if (efekAktif != null && efekAktif.activeSelf && targetMengikuti != null)
        {
            efekAktif.transform.position = targetMengikuti.position + new Vector3(0, 0.15f, 0);
        }
    }

    public void SetTutorialDinamis(List<string> daftarTeksBaru, List<Transform> ruteBaru)
    {
        instruksiAktif = new List<string>(daftarTeksBaru);
        rutePetunjukAktif = new List<Transform>(ruteBaru);

        stepSekarang = 0;
        PerbaruiTampilanTutorial();
    }

    public int AmbilStepSekarang()
    {
        return stepSekarang;
    }

    public void LaporSelesai(int stepYangSelesai)
    {
        Debug.Log($"[TutorialManager] Menerima laporan sukses untuk step: {stepYangSelesai}. Step saat ini: {stepSekarang}");

        if (stepSekarang == stepYangSelesai)
        {
            stepSekarang++;
            Debug.Log($"[TutorialManager] Sukses! Step maju ke: {stepSekarang}");

            // Panggil fungsi pembaru tampilan di sini
            PerbaruiTampilanTutorial();
        }
        else
        {
            Debug.LogWarning($"[TutorialManager] Laporan step {stepYangSelesai} diabaikan karena tidak sesuai dengan stepSekarang ({stepSekarang})");
        }
    }

    // Fungsi klik/interact universal untuk memajukan langkah TV
    public void InteractObjek()
    {
        LaporSelesai(stepSekarang);
    }

    private void PerbaruiTampilanTutorial()
    {
        if (teksTutorial == null) return;

        if (instruksiAktif != null && stepSekarang < instruksiAktif.Count)
        {
            teksTutorial.text = instruksiAktif[stepSekarang];
        }
        else
        {
            teksTutorial.text = "Bagus! Semua langkah selesai, silakan nikmati hasil simulasimu!";
            if (efekAktif != null) efekAktif.SetActive(false);
            return;
        }

        if (rutePetunjukAktif != null && stepSekarang < rutePetunjukAktif.Count)
        {
            targetMengikuti = rutePetunjukAktif[stepSekarang];
            UpdateGlitterVisual(true);
        }
        else
        {
            targetMengikuti = null;
            UpdateGlitterVisual(false);
        }
    }

    private void UpdateGlitterVisual(bool nyala)
    {
        if (efekAktif == null) return;

        if (nyala && targetMengikuti != null)
        {
            efekAktif.SetActive(true);
            ParticleSystem ps = efekAktif.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }
        else
        {
            efekAktif.SetActive(false);
        }
    }
}