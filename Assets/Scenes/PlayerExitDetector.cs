using UnityEngine;

public class PlayerExitDetector : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            RaceManager manager = FindObjectOfType<RaceManager>();
            if (manager != null)
            {
                manager.ReenterVehicle();
            }
        }
    }
}