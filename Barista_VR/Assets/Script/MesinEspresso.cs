using UnityEngine;


public class MesinEspresso : MonoBehaviour
{
    [Header("Socket Mesin")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketPortafilter; // Socket tempat portafilter nempel
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketGelas;       // Socket tempat gelas ditaruh

    // Fungsi ini akan dipanggil otomatis setiap kali ada benda masuk ke socket
    public void CekDanBikinKopi()
    {
        // Cek: Apakah Portafilter ADA isinya? DAN Gelas ADA isinya?
        if (socketPortafilter.hasSelection && socketGelas.hasSelection)
        {
            // Ambil data objek gelas yang sedang menempel di socket
            GameObject objekGelas = socketGelas.firstInteractableSelected.transform.gameObject;

            // Cari script GelasKopi di gelas tersebut dan suruh isi air
            GelasKopi scriptGelas = objekGelas.GetComponent<GelasKopi>();
            if (scriptGelas != null)
            {
                scriptGelas.MulaiIsiAir();
            }
        }
    }
}