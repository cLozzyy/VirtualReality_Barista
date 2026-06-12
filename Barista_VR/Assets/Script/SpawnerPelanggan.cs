using UnityEngine;

public class SpawnerPelanggan : MonoBehaviour
{
    public GameObject prefabPelanggan;
    public Transform titikSpawn;
    public int targetPelanggan = 2;

    private int jumlahPelangganSelesai = 0;
    private GameObject pelangganSaatIni;

    void Start()
    {
        SpawnPelanggan();
    }

    public void SpawnPelanggan()
    {
        if (jumlahPelangganSelesai < targetPelanggan && pelangganSaatIni == null)
        {
            pelangganSaatIni = Instantiate(prefabPelanggan, titikSpawn.position, titikSpawn.rotation);

            SistemPelanggan sp = pelangganSaatIni.GetComponent<SistemPelanggan>();
            if (sp != null)
            {
                sp.spawner = this;
            }

            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.ResetTutorial();
            }
        }
    }

    public void HitungSelesai()
    {
        // KUNCI UTAMA: Kosongkan dulu slot pelanggan saat ini biar spawner tau dia udah pergi
        pelangganSaatIni = null;

        jumlahPelangganSelesai++;

        if (jumlahPelangganSelesai < targetPelanggan)
        {
            SpawnPelanggan();
        }
        else
        {
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.TampilkanSelesai("Selamat kamu sudah menyelesaikan game ini !");
            }
        }
    }
}