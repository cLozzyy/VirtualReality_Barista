using UnityEngine;
using System.Collections;

public class KeluarPelanggan : MonoBehaviour
{
    public Transform titikKeluar;
    public float kecepatanJalan = 2.5f;

    public void JalankanKeluar() { StartCoroutine(ProsesKeluar()); }

    private IEnumerator ProsesKeluar()
    {
        var sistem = GetComponent<SistemPelanggan>();
        if (sistem != null && sistem.teksBalonPelanggan != null)
            sistem.teksBalonPelanggan.text = "Terima kasih!";

        yield return new WaitForSeconds(2.0f);

        if (titikKeluar != null)
        {
            while (Vector3.Distance(transform.position, titikKeluar.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, titikKeluar.position, kecepatanJalan * Time.deltaTime);
                transform.LookAt(new Vector3(titikKeluar.position.x, transform.position.y, titikKeluar.position.z));
                yield return null;
            }
        }
        gameObject.SetActive(false);
    }
}