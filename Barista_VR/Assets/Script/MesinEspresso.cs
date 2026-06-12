using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MesinEspresso : MonoBehaviour
{
    [Header("Socket Mesin")]
    public XRSocketInteractor socketPortafilter;
    public XRSocketInteractor socketGelas;

    [Header("Audio Efek")]
    public AudioSource audioMesin;

    // Fungsi ini dipanggil otomatis lewat Event Socket (Select Entered) milik KEDUA socket di atas
    // --- UPDATE FUNGSI INI DI DALAM MesinEspresso.cs ---
    public void CekDanBikinKopi()
    {
        if (socketPortafilter.hasSelection && socketGelas.hasSelection)
        {
            GameObject objekPortafilter = socketPortafilter.firstInteractableSelected.transform.gameObject;
            PortafilterKopi scriptPF = objekPortafilter.GetComponent<PortafilterKopi>();

            if (scriptPF != null && scriptPF.sudahAdaKopi)
            {
                GameObject objekGelas = socketGelas.firstInteractableSelected.transform.gameObject;
                GelasKopi scriptGelas = objekGelas.GetComponent<GelasKopi>();

                // KUNCI ANTI DOUBLE-TRIGGER: Pastikan gelas belum penuh, DAN mesin tidak sedang menyeduh!
                if (scriptGelas != null && !scriptGelas.sudahPenuh && !scriptGelas.sedangIsi)
                {
                    // Lapor ke TV untuk maju ke teks "Tunggu kopi diekstrak" HANYA SEKALI
                    if (TutorialManager.instance != null)
                    {
                        int stepPasangSelesai = TutorialManager.instance.AmbilStepSekarang();
                        TutorialManager.instance.LaporSelesai(stepPasangSelesai);
                    }

                    PutarSuaraMesin(scriptGelas.durasiIsi);
                    scriptGelas.MulaiIsiAir();
                }
            }
            else
            {
                Debug.LogWarning("MesinEspresso: Portafilter nempel, tapi KOSONG!");
            }
        }
    }

    private void PutarSuaraMesin(float durasi)
    {
        if (audioMesin != null)
        {
            if (!audioMesin.isPlaying)
            {
                audioMesin.Play();
                CancelInvoke("MatikanSuaraMesin");
                Invoke("MatikanSuaraMesin", durasi);
            }
        }
    }

    public void MatikanSuaraMesin()
    {
        if (audioMesin != null && audioMesin.isPlaying)
        {
            audioMesin.Stop();
        }
    }
}