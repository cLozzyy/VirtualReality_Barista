using UnityEngine;

public class SistemPelanggan : MonoBehaviour
{
    [Header("Pergerakan")]
    public Transform titikKasir;
    public float kecepatan = 1.5f;

    [Header("UI Resep")]
    public GameObject canvasResep;

    [Header("Animation")]
    public Animator animator; 

    private bool sudahDiKasir = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (!sudahDiKasir && titikKasir != null)
        {
            MulaiBerjalan();
        }
        else
        {
            BerhentiBerjalan();
        }
    }

    void MulaiBerjalan()
    {
        transform.position = Vector3.MoveTowards(transform.position, titikKasir.position, kecepatan * Time.deltaTime);
        
        // Putar karakter menghadap titik tujuan agar tidak jalan mundur/samping
        Vector3 targetDirection = titikKasir.position - transform.position;
        if (targetDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 10f * Time.deltaTime);
        }

        // Set parameter Animator agar transisi ke Rig_walk
        if (animator != null)
        {
            animator.SetBool("isJalan", true);
        }

        if (Vector3.Distance(transform.position, titikKasir.position) < 0.1f)
        {
            sudahDiKasir = true;
            Debug.Log("Pelanggan sudah menunggu pesanan.");
        }
    }

    void BerhentiBerjalan()
    {
        // Set parameter Animator agar transisi balik ke Rig_idle
        if (animator != null)
        {
            animator.SetBool("isJalan", false);
        }
    }

    public void TampilkanResep()
    {
        if (sudahDiKasir && canvasResep != null)
        {
            canvasResep.SetActive(true);
            Debug.Log("Resep ditampilkan!");
            TutorialManager.instance.LaporSelesai(0);
        }
    }
}