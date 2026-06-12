using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PiringSaji : MonoBehaviour
{
    public XRSocketInteractor socketPiring;

    void Start()
    {
        if (socketPiring == null) socketPiring = GetComponent<XRSocketInteractor>();
        socketPiring.selectEntered.AddListener(OnGelasSuksesNempel);
    }

    private void OnGelasSuksesNempel(SelectEnterEventArgs args)
    {
        GameObject objekGelas = args.interactableObject.transform.gameObject;
        SistemPelanggan pelanggan = Object.FindAnyObjectByType<SistemPelanggan>();

        if (pelanggan != null)
        {
            pelanggan.TerimaSajianKopi(objekGelas);

            // RESET OTOMATIS BERDASARKAN TAG ASLI
            ResetObjekBerdasarkanTag("Cup");
            ResetObjekBerdasarkanTag("Portafilter");

            socketPiring.enabled = false;
            Invoke("NyalakanSocketPenuh", 1f);
        }
    }

    private void ResetObjekBerdasarkanTag(string namaTag)
    {
        GameObject[] daftarObjek = GameObject.FindGameObjectsWithTag(namaTag);
        foreach (GameObject obj in daftarObjek)
        {
            KembaliKeAwal resetter = obj.GetComponent<KembaliKeAwal>();
            if (resetter != null) resetter.ResetBarang();
        }
    }

    private void NyalakanSocketPenuh() { socketPiring.enabled = true; }
}