using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour
{
    public bool full;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            full = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            full = false;
    }
}