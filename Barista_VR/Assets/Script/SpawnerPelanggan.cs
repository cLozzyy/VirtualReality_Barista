using UnityEngine;

public class SpawnerPelanggan : MonoBehaviour
{
    public GameObject prefabPelanggan;
    public Transform titikSpawn;
    private int jumlahPelangganSelesai = 0;
    public int targetPelanggan = 2;

    void Start() { SpawnPelanggan(); }

    public void SpawnPelanggan()
    {
        if (jumlahPelangganSelesai < targetPelanggan)
        {
            GameObject p = Instantiate(prefabPelanggan, titikSpawn.position, titikSpawn.rotation);
            p.GetComponent<SistemPelanggan>().spawner = this;

            // Reset Tutorial TV setiap pelanggan baru datang
            if (TutorialManager.instance != null) TutorialManager.instance.ResetTutorial();
        }
    }

    public void HitungSelesai()
    {
        jumlahPelangganSelesai++;
        if (jumlahPelangganSelesai < targetPelanggan) SpawnPelanggan();
        else TutorialManager.instance.TampilkanSelesai("Selamat kamu sudah menyelesaikan game ini !");
    }
}