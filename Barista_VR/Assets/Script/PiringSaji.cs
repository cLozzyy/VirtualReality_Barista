using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PiringSaji : MonoBehaviour
{
    public XRSocketInteractor socketPiring; // Tarik SocketCupFilled ke sini
    public SistemPelanggan pelanggan;       // Tarik objek Pelanggan ke sini

    public void CekPesananMasuk()
    {
        if (socketPiring.hasSelection)
        {
            // Ambil data gelas yang ditaruh di piring
            GameObject objekGelas = socketPiring.firstInteractableSelected.transform.gameObject;
            GelasKopi scriptGelas = objekGelas.GetComponent<GelasKopi>();

            // Cek: Apakah itu beneran gelas? DAN Apakah kopinya penuh?
            if (scriptGelas != null && scriptGelas.sudahPenuh)
            {
                Debug.Log("Pesanan disajikan!");

                // 1. Pelanggan bilang terima kasih
                if (pelanggan != null) pelanggan.PesananSelesai();

                // 2. Lepas paksa gelas dari genggaman socket piring (biar gak nyangkut pas di-reset)
                socketPiring.enabled = false;

                // 3. Kosongkan isi gelas
                scriptGelas.KosongkanGelas();

                // 4. Terbangkan gelas balik ke tempat asal
                KembaliKeAwal resetScript = objekGelas.GetComponent<KembaliKeAwal>();
                if (resetScript != null)
                {
                    resetScript.ResetBarang();
                }

                // 5. Nyalakan lagi socket piringnya sedetik kemudian untuk pesanan berikutnya
                Invoke("NyalakanSocket", 1f);
            }
            else
            {
                Debug.LogWarning("Ditolak! Gelas masih kosong atau salah barang.");
            }
        }
    }

    private void NyalakanSocket()
    {
        socketPiring.enabled = true;
    }
}