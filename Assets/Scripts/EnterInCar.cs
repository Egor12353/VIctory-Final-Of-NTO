using System.Collections;
using UnityEngine;
using Valve.VR;

public class EnterInCar : MonoBehaviour
{
    [Header("VR Input")]
    [SerializeField] private SteamVR_Action_Boolean enterButton;
    [SerializeField] private SteamVR_ActionSet carSet;
    [SerializeField] private SteamVR_ActionSet defaultSet;

    [Header("References")]
    [SerializeField] private Transform seat;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private TeleportManager teleportManager;

    [Header("Vehicle Type")]
    public bool isInTruck; // true - грузовик, false - автобус

    [Header("State")]
    public bool inDrive = false;

    private bool enterButtonPressed = false;
    private Transform player;

    private void FixedUpdate()
    {
        if (!inDrive) return;

        if (enterButton.GetStateDown(SteamVR_Input_Sources.RightHand) && !enterButtonPressed)
        {
            ExitVehicle();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand") && enterButton.GetStateDown(SteamVR_Input_Sources.RightHand) && !enterButtonPressed)
        {
            EnterVehicle(other.transform.root);
        }
    }

    private void EnterVehicle(Transform playerTransform)
    {
        enterButtonPressed = true;
        teleportManager.enabled = false;
        StartCoroutine(Unpress());

        player = playerTransform;
        player.position = seat.position;
        player.SetParent(seat);
        player.localEulerAngles = Vector3.zero;

        inDrive = true;
        carSet.Activate(SteamVR_Input_Sources.Any);
    }

    private void ExitVehicle()
    {
        enterButtonPressed = true;
        StartCoroutine(Unpress());

        player.SetParent(null);
        player.position = exitPoint.position;
        player.rotation = exitPoint.rotation;

        inDrive = false;
        teleportManager.enabled = true;
        carSet.Deactivate(SteamVR_Input_Sources.Any);
    }

    private IEnumerator Unpress()
    {
        yield return new WaitForSeconds(0.2f);
        enterButtonPressed = false;
    }
}