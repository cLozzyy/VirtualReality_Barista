using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SistemPelanggan : MonoBehaviour
{
    public enum TipeKopi { HotAmericano, IceAmericano }
    public enum LevelGula { NoSugar, LessSugar, NormalSugar, ExtraSugar }
    public SpawnerPelanggan spawner;

    [Header("Pesanan Pelanggan (Otomatis Random)")]
    public TipeKopi kopiDipesan;
    public LevelGula gulaDipesan;

    [Header("UI Teks Resep Kopi (Monitor Kasir)")]
    public TextMeshProUGUI textResepTunggal;
    public GameObject panelCanvasResep;

    [Header("UI Teks Balon Ucapan (Di Atas Kepala NPC)")]
    public TextMeshPro teksBalonPelanggan;

    [Header("Referensi Objek Bar (Untuk Rute Glitter)")]
    public Transform objekPortafilter;
    public Transform objekSendokKopi;
    public Transform objekToplesKopi;
    public Transform objekTamperKopi;
    public Transform objekMesinEspresso;
    public Transform objekEsBatu;
    public Transform objekPiringSaji;
    public Transform objekGula;

    [Header("Status Pelayanan")]
    public bool pesananSelesai = false;
    private bool sudahPesan = false;

    [Header("KODE AWAL: Pergerakan ke Kasir")]
    public Transform titikKasir;
    public float kecepatanJalan = 2f;
    private bool sudahSampaiKasir = false;

    void Start()
    {
        pesananSelesai = false;
        sudahSampaiKasir = false;
        sudahPesan = false;

        // Cari titik kasir jika belum ada
        if (titikKasir == null)
        {
            GameObject go = GameObject.FindWithTag("TitikKasir");
            if (go != null) titikKasir = go.transform;
        }

        if (panelCanvasResep != null) panelCanvasResep.SetActive(false);

        // Langsung panggil GenerateOrder setelah sedikit jeda agar tidak crash
        Invoke("GenerateRandomOrder", 1.0f);
    }

    void Update()
    {
        if (titikKasir != null && !sudahSampaiKasir)
        {
            transform.position = Vector3.MoveTowards(transform.position, titikKasir.position, kecepatanJalan * Time.deltaTime);

            if (Vector3.Distance(transform.position, titikKasir.position) < 0.1f)
            {
                sudahSampaiKasir = true;
                Debug.Log("[Sistem Pelanggan] Sudah sampai di depan kasir.");
            }
        }
    }

    public void GenerateRandomOrder()
    {
        if (sudahSampaiKasir && !sudahPesan)
        {
            pesananSelesai = false;
            sudahPesan = true;

            kopiDipesan = (TipeKopi)Random.Range(0, 2);
            gulaDipesan = (LevelGula)Random.Range(0, 4);

            string namaKopiPelanggan = (kopiDipesan == TipeKopi.HotAmericano) ? "Hot Americano" : "Ice Americano";
            string namaGulaPelanggan = "";

            switch (gulaDipesan)
            {
                case LevelGula.NoSugar: namaGulaPelanggan = "tanpa gula"; break;
                case LevelGula.LessSugar: namaGulaPelanggan = "sedikit gula"; break;
                case LevelGula.NormalSugar: namaGulaPelanggan = "gula normal"; break;
                case LevelGula.ExtraSugar: namaGulaPelanggan = "banyak gula"; break;
            }

            if (teksBalonPelanggan != null)
            {
                teksBalonPelanggan.text = $"Gw mau {namaKopiPelanggan}\n{namaGulaPelanggan} dong !";
            }

            UpdateVisualResep();
            SusunTutorialDanRuteDinamis();
        }
    }

    private void SusunTutorialDanRuteDinamis()
    {
        if (TutorialManager.instance == null) return;

        List<string> teksTVDinamis = new List<string>();
        List<Transform> ruteGlitterDinamis = new List<Transform>();

        teksTVDinamis.Add("Ambil Portafilter dan letakkan di atas timbangan.");
        ruteGlitterDinamis.Add(objekPortafilter);

        // --- STEP DIGABUNG ---
        // Step 1: Ambil sendok DAN langsung scoop kopi
        teksTVDinamis.Add("Ambil sendok, ambil bubuk kopi, dan tuang ke Portafilter hingga 15g.");
        ruteGlitterDinamis.Add(objekSendokKopi);

        // Step 2: Tamper
        teksTVDinamis.Add("Ambil Tamper, tekan bubuk kopi di dalam Portafilter hingga padat rata.");
        ruteGlitterDinamis.Add(objekTamperKopi);

        teksTVDinamis.Add("Pasang Portafilter ke Group Head Mesin Espresso dan letakkan GELAS KOSONG di bawahnya.");
        ruteGlitterDinamis.Add(objekMesinEspresso);

        teksTVDinamis.Add("Nyalakan mesin espresso dan tunggu sampai gelas terisi penuh dengan kopi.");
        ruteGlitterDinamis.Add(objekMesinEspresso);

        // --- URUTAN BARU 1: GULA DIMINTA DULUAN SETELAH KOPI PENUH ---
        if (gulaDipesan != LevelGula.NoSugar)
        {
            int jumlahScoopGula = 0;
            if (gulaDipesan == LevelGula.LessSugar) jumlahScoopGula = 1;
            else if (gulaDipesan == LevelGula.NormalSugar) jumlahScoopGula = 2;
            else if (gulaDipesan == LevelGula.ExtraSugar) jumlahScoopGula = 3;

            teksTVDinamis.Add($"Racikan butuh rasa manis! Tuangkan gula pasir dari toples sebanyak {jumlahScoopGula} Scoop ke dalam gelas kopi.");
            ruteGlitterDinamis.Add(objekGula);
        }

        // --- URUTAN BARU 2: ES BATU DIMINTA SETELAH GULA ---
        if (kopiDipesan == TipeKopi.IceAmericano)
        {
            teksTVDinamis.Add("Sentuhan terakhir! Bawa gelas tersebut dan dekatkan ke arah Cooler Box untuk menambahkan Es Batu.");
            ruteGlitterDinamis.Add(objekEsBatu);
        }

        // --- TERAKHIR: SAJIKAN ---
        teksTVDinamis.Add("Sempurna! Sekarang bawa gelas kopi yang sudah lengkap rasanya ke Piring Saji (Plate) depan pelanggan.");
        ruteGlitterDinamis.Add(objekPiringSaji);

        TutorialManager.instance.SetTutorialDinamis(teksTVDinamis, ruteGlitterDinamis);
    }

    public bool ApakahResepSudahLengkap(GelasKopi scriptGelas)
    {
        if (scriptGelas == null) return false;
        if (!scriptGelas.sudahPenuh) return false;

        bool esCocok = false;
        if (kopiDipesan == TipeKopi.IceAmericano && scriptGelas.sudahAdaEs) esCocok = true;
        if (kopiDipesan == TipeKopi.HotAmericano && !scriptGelas.sudahAdaEs) esCocok = true;

        int targetScoopGula = 0;
        if (gulaDipesan == LevelGula.LessSugar) targetScoopGula = 1;
        else if (gulaDipesan == LevelGula.NormalSugar) targetScoopGula = 2;
        else if (gulaDipesan == LevelGula.ExtraSugar) targetScoopGula = 3;

        bool gulaCocok = (scriptGelas.jumlahScoopGula == targetScoopGula);

        return (esCocok && gulaCocok);
    }

    public void TerimaSajianKopi(GameObject objekGelas)
    {
        GelasKopi scriptGelas = objekGelas.GetComponent<GelasKopi>();
        if (scriptGelas == null) return;

        Debug.Log("Customer: Mantap! Gelas masuk piring saji dengan komposisi sempurna!");
        pesananSelesai = true;
        sudahPesan = false;

        if (teksBalonPelanggan != null)
        {
            teksBalonPelanggan.text = "Thank You !";
        }

        scriptGelas.KosongkanGelas();

        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.InteractObjek(); // Selesaikan step terakhir
        }

        if (panelCanvasResep != null) panelCanvasResep.SetActive(false);
    }

    private void UpdateVisualResep()
    {
        if (panelCanvasResep != null) panelCanvasResep.SetActive(true);
        if (textResepTunggal == null) return;

        string namaMenu = (kopiDipesan == TipeKopi.HotAmericano) ? "HOT AMERICANO" : "ICE AMERICANO";
        string infoGula = "";

        switch (gulaDipesan)
        {
            case LevelGula.NoSugar: infoGula = " (No Sugar)"; break;
            case LevelGula.LessSugar: infoGula = " (Less Sugar)"; break;
            case LevelGula.NormalSugar: infoGula = " (Normal Sugar)"; break;
            case LevelGula.ExtraSugar: infoGula = " (Extra Sugar)"; break;
        }

        string totalKalimatResep = "MENU: " + namaMenu + infoGula + "\n\nResep Pembuatan:\n";
        totalKalimatResep += "- Grind kopi & tuang ke Portafilter (15g)\n- Tekan menggunakan Tamper\n- Ekstrak espresso pake mesin\n";

        // Ubah urutan di UI Kasir biar sesuai sama TV
        switch (gulaDipesan)
        {
            case LevelGula.NoSugar: totalKalimatResep += "- Gula: 0 Scoop (No Sugar)\n"; break;
            case LevelGula.LessSugar: totalKalimatResep += "- Gula: 1 Scoop (Less Sugar)\n"; break;
            case LevelGula.NormalSugar: totalKalimatResep += "- Gula: 2 Scoop (Normal)\n"; break;
            case LevelGula.ExtraSugar: totalKalimatResep += "- Gula: 3 Scoop (Extra)\n"; break;
        }

        totalKalimatResep += (kopiDipesan == TipeKopi.IceAmericano) ? "- Tambah Es Batu dari Cooler Box\n" : "- Gelas Polos Hangat (Tanpa Es)\n";
        totalKalimatResep += "\nSajikan ke Piring Saji!";
        textResepTunggal.text = totalKalimatResep;
    }

    // Tambahkan di SistemPelanggan.cs
    public void SelesaikanPesanan()
    {
        StartCoroutine(ProsesKeluar());
    }

    private IEnumerator ProsesKeluar()
    {
        // 1. Bilang terima kasih
        if (teksBalonPelanggan != null) teksBalonPelanggan.text = "Terima kasih!";
        yield return new WaitForSeconds(1.5f);

        // 2. Jalan ke Titik Keluar (Tambahkan logika ini jika ingin dia jalan balik)
        // Pastikan lu sudah set 'titikKeluar' di Inspector prefab Pelanggan
        // Jika tidak ada titik keluar, dia akan langsung hilang.

        // 3. Panggil Spawner (SANGAT PENTING)
        if (spawner != null)
        {
            spawner.HitungSelesai(); // Ini yang memicu pelanggan berikutnya
        }

        // 4. Reset Tutorial
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.ResetTutorial();
        }

        gameObject.SetActive(false);
    }
}