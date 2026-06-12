using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FridgeHandle : MonoBehaviour
{
    public Transform playerHand;
    public HingeJoint door;

    void Update()
    {
        if (playerHand != null)
        {
            // hitung arah tangan terhadap engsel
            // lalu putar pintu
        }
    }
}