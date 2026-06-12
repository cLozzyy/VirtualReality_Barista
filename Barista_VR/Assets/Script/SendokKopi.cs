using UnityEngine;

public class SendokKopi : MonoBehaviour
{
    public enum IsiSendok { Kosong, Kopi, Gula }

    [Header("Visual di Sendok")]
    public GameObject gundukanKopi;
    public GameObject gundukanGula;

    [Header("Status Sendok")]
    public IsiSendok statusSekarang = IsiSendok.Kosong;

    private void Start()
    {
        if (gundukanKopi != null) gundukanKopi.SetActive(false);
        if (gundukanGula != null) gundukanGula.SetActive(false);
        statusSekarang = IsiSendok.Kosong;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. AMBIL BAHAN
        if (statusSekarang == IsiSendok.Kosong)
        {
            if (other.CompareTag("ToplesKopi")) AmbilBahan(IsiSendok.Kopi);
            else if (other.CompareTag("ToplesGula")) AmbilBahan(IsiSendok.Gula);
        }
        // 2. TUANG BAHAN
        else
        {
            // Tuang Kopi ke Portafilter
            if (statusSekarang == IsiSendok.Kopi && other.CompareTag("Portafilter"))
            {
                TuangKopi(other.gameObject);
            }
            // Tuang Gula ke Gelas (Ganti CompareTag dengan pengecekan Script GelasKopi)
            else if (statusSekarang == IsiSendok.Gula)
            {
                GelasKopi gelas = other.GetComponent<GelasKopi>();
                if (gelas != null) TuangGula(gelas);
            }
        }
    }

    private void AmbilBahan(IsiSendok bahanBaru)
    {
        statusSekarang = bahanBaru;
        gundukanKopi.SetActive(bahanBaru == IsiSendok.Kopi);
        gundukanGula.SetActive(bahanBaru == IsiSendok.Gula);
        Debug.Log("Sendok: Berhasil mengambil " + bahanBaru);
    }

    private void TuangKopi(GameObject areaTuang)
    {
        gundukanKopi.SetActive(false);
        statusSekarang = IsiSendok.Kosong;

        PortafilterKopi pf = areaTuang.GetComponent<PortafilterKopi>();
        if (pf != null) pf.TerimaKopi();
    }

    private void TuangGula(GelasKopi gelas)
    {
        // Matikan visual & kosongkan status sendok
        gundukanGula.SetActive(false);
        statusSekarang = IsiSendok.Kosong;

        // PANGGIL FUNGSI TAMBAH GULA DI GELAS KOPI
        gelas.TambahGula();

        Debug.Log("Sendok: Gula berhasil dituang ke gelas!");
    }
}