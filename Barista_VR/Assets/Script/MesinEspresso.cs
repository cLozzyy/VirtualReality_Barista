using UnityEngine;

// Baris ini bikin Unity otomatis nambahin AudioSource kalau di objeknya belum ada
[RequireComponent(typeof(AudioSource))]
public class MesinEspresso : MonoBehaviour
{
    [Header("Socket Mesin")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketPortafilter;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketGelas;

    // Variabel AudioClip dihapus biar Inspector lebih bersih
    // Kita cuma butuh reference ke AudioSource-nya aja
    private AudioSource suaraMesin;

    private void Awake()
    {
        // Otomatis ngambil komponen AudioSource yang nempel di objek mesin ini
        suaraMesin = GetComponent<AudioSource>();
    }

    public void CekDanBikinKopi()
    {
        if (socketPortafilter.hasSelection && socketGelas.hasSelection)
        {
            GameObject objekPortafilter = socketPortafilter.firstInteractableSelected.transform.gameObject;
            GameObject objekGelas = socketGelas.firstInteractableSelected.transform.gameObject;

            PortafilterKopi scriptPorta = objekPortafilter.GetComponent<PortafilterKopi>();

            if (scriptPorta != null && scriptPorta.portaFilter_Coffee != null && scriptPorta.portaFilter_Coffee.activeSelf)
            {
                // Langsung play aja AudioSource-nya
                if (suaraMesin != null)
                {
                    suaraMesin.Play();
                }

                GelasKopi scriptGelas = objekGelas.GetComponent<GelasKopi>();
                if (scriptGelas != null)
                {
                    scriptGelas.MulaiIsiAir();
                }

                Debug.Log("Mesin menyala! Mengekstrak kopi...");
            }
            else
            {
                Debug.Log("Mesin tidak menyala: Portafilter belum diisi bubuk kopi!");
            }
        }
    }

    public void HentikanMesin()
    {
        if (suaraMesin != null && suaraMesin.isPlaying)
        {
            suaraMesin.Stop();
        }
    }
}