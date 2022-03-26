using Game.Player;
using UnityEngine;

public class damageTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPSController.OnTakeDamage(15);
        }
    }
}
