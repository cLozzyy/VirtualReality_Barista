using UnityEngine;
using TMPro; // Wajib ditambahkan agar Unity mengenali TextMeshPro

public class PortafilterKopi : MonoBehaviour
{
    [Header("Visual Kopi Portafilter")]
    public GameObject portaFilter_Coffee;

    [Header("Layar Digital")]
    // Ini tempat untuk masukin objek tulisan di timbangan
    public TextMeshPro teksTimbangan;

    public float totalBeratKopi = 0f;

    public void TerimaKopi()
    {
        if (portaFilter_Coffee != null)
        {
            portaFilter_Coffee.SetActive(true); // Menyalakan kopi
            totalBeratKopi += 15.0f;            // Menambah berat 15 gram

            // Mengubah tulisan di layar timbangan
            if (teksTimbangan != null)
            {
                // ToString("F1") gunanya biar angkanya ada 1 desimal (contoh: 15.0 g)
                teksTimbangan.text = totalBeratKopi.ToString("F1") + " g";
            }
            else
            {
                Debug.LogWarning("Objek Layar Timbangan belum dimasukkan ke script!");
            }
                
            TutorialManager.instance.LaporSelesai(2);
            Debug.Log("Sukses! Kopi dituang. Total: " + totalBeratKopi + " gram");
        }
        else
        {
            Debug.LogError("WADUH! Objek 'Porta filter_Coffee' belum dimasukkan!");
        }
    }
}