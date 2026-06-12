using UnityEngine;
using System.Collections;

public class GelasKopi : MonoBehaviour
{
    [Header("Visual Air Kopi")]
    public Transform pivotAirKopi;
    private MeshRenderer rendererAir;

    [Header("Visual Es Batu")]
    public GameObject visualEsBatu;
    public bool sudahAdaEs = false;

    [Header("Visual & Data Gula")]
    public int jumlahScoopGula = 0;

    [Header("Pengaturan Pengisian")]
    public float targetPenuhY = 1f;
    public float durasiIsi = 3f;

    public bool sedangIsi = false;
    public bool sudahPenuh = false;

    void Start()
    {
        rendererAir = pivotAirKopi.GetComponentInChildren<MeshRenderer>();
        if (rendererAir != null) rendererAir.enabled = false;

        Vector3 skalaAwal = pivotAirKopi.localScale;
        skalaAwal.y = 0f;
        pivotAirKopi.localScale = skalaAwal;

        if (visualEsBatu != null) visualEsBatu.SetActive(false);

        sudahAdaEs = false;
        jumlahScoopGula = 0;
        sedangIsi = false;
        sudahPenuh = false;
    }

    public void MulaiIsiAir()
    {
        if (!sedangIsi && !sudahPenuh)
        {
            StartCoroutine(ProsesIsi());
        }
    }

    private IEnumerator ProsesIsi()
    {
        sedangIsi = true;
        Debug.Log("Mesin menyala! Mengisi kopi...");

        if (rendererAir != null) rendererAir.enabled = true;

        float waktu = 0;
        Vector3 skalaTarget = new Vector3(pivotAirKopi.localScale.x, targetPenuhY, pivotAirKopi.localScale.z);

        while (waktu < durasiIsi)
        {
            waktu += Time.deltaTime;
            float persentase = waktu / durasiIsi;
            pivotAirKopi.localScale = Vector3.Lerp(new Vector3(skalaTarget.x, 0f, skalaTarget.z), skalaTarget, persentase);
            yield return null;
        }

        sudahPenuh = true;
        sedangIsi = false;
        Debug.Log("Gelas Penuh!");

        // --- CEK APAKAH PERLU SKIP STEP GULA ---
        if (TutorialManager.instance != null)
        {
            SistemPelanggan pelanggan = Object.FindAnyObjectByType<SistemPelanggan>();
            if (pelanggan != null && pelanggan.gulaDipesan == SistemPelanggan.LevelGula.NoSugar)
            {
                Debug.Log("[GelasKopi] Pesanan No Sugar. Skip ke langkah selanjutnya.");
                TutorialManager.instance.InteractObjek(); // Lanjut ke Es atau Plate
            }
            else
            {
                TutorialManager.instance.InteractObjek(); // Lanjut ke perintah isi gula
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // A. DETEKSI COOLER BOX
        if (other.CompareTag("CoolerBox") && sudahPenuh && !sudahAdaEs)
        {
            if (TutorialManager.instance != null)
            {
                string teksTV = TutorialManager.instance.teksTutorial.text;
                if (teksTV.Contains("Es Batu") || teksTV.Contains("Cooler"))
                {
                    TambahEsBatu();
                }
            }
        }

        // B. DETEKSI SENDOK (Gunakan .Contains agar tidak perlu daftar Tag baru)
        if (other.name.Contains("Sendok") || other.name.Contains("Spoon"))
        {
            if (TutorialManager.instance != null)
            {
                string teksTV = TutorialManager.instance.teksTutorial.text;
                if (teksTV.Contains("gula") || teksTV.Contains("Scoop"))
                {
                    TambahGula();
                }
            }
        }
    }

    private void TambahEsBatu()
    {
        if (visualEsBatu != null)
        {
            visualEsBatu.SetActive(true);
            sudahAdaEs = true;
            Debug.Log("GelasKopi: Es Batu masuk!");

            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.InteractObjek();
            }
        }
    }

    public void TambahGula()
    {
        SistemPelanggan pelanggan = Object.FindAnyObjectByType<SistemPelanggan>();
        if (pelanggan == null) return;

        // Jika No Sugar, jangan tambahkan apapun
        if (pelanggan.gulaDipesan == SistemPelanggan.LevelGula.NoSugar) return;

        int targetScoop = 0;
        if (pelanggan.gulaDipesan == SistemPelanggan.LevelGula.LessSugar) targetScoop = 1;
        else if (pelanggan.gulaDipesan == SistemPelanggan.LevelGula.NormalSugar) targetScoop = 2;
        else if (pelanggan.gulaDipesan == SistemPelanggan.LevelGula.ExtraSugar) targetScoop = 3;

        jumlahScoopGula++;
        Debug.Log($"[GelasKopi] Gula Masuk: {jumlahScoopGula}/{targetScoop} Scoop.");

        if (jumlahScoopGula == targetScoop)
        {
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.InteractObjek();
                Debug.Log("[GelasKopi] Takaran gula pas! TV maju.");
            }
        }
    }

    public void KosongkanGelas()
    {
        sudahPenuh = false;
        sedangIsi = false;
        sudahAdaEs = false;
        jumlahScoopGula = 0;

        if (rendererAir != null) rendererAir.enabled = false;
        if (visualEsBatu != null) visualEsBatu.SetActive(false);

        Vector3 skalaAwal = pivotAirKopi.localScale;
        skalaAwal.y = 0f;
        pivotAirKopi.localScale = skalaAwal;
    }
}