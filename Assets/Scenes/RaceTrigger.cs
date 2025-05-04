using UnityEngine;

public class RaceTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaceManager manager = FindObjectOfType<RaceManager>();
            if (manager != null)
            {
                manager.ReportTrigger(GetComponent<Collider>());
            }
        }
    }
}