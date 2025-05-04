using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConesTrigger : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    [Header("Settings")]
    public float conePushForce = 5f;
    public int firstCanvasThreshold = 10;
    public int secondCanvasThreshold = 25;
    public float fadeDuration = 1f;

    [Header("UI References")]
    public Canvas firstWarningCanvas;
    public Canvas secondWarningCanvas;

    private int conesHitCount;
    private HashSet<GameObject> hitCones = new HashSet<GameObject>();
    private bool restarting;
    private Coroutine firstCanvasFade;
    private Coroutine secondCanvasFade;

    private void Start()
    {
        // Отключаем оба канваса при старте
        firstWarningCanvas.enabled = false;
        secondWarningCanvas.enabled = false;
    }

    private void FixedUpdate()
    {
        CheckWheelContact(frontLeftWheelCollider);
        CheckWheelContact(frontRightWheelCollider);
        CheckWheelContact(rearLeftWheelCollider);
        CheckWheelContact(rearRightWheelCollider);
    }

    private void CheckWheelContact(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Conus") && !hitCones.Contains(hitObject))
            {
                hitCones.Add(hitObject);
                HandleConeCollision(hitObject);
            }
        }
    }

    private void HandleConeCollision(GameObject cone)
    {
        Rigidbody coneRb = cone.GetComponent<Rigidbody>();
        if (coneRb != null)
        {
            coneRb.AddForce(Vector3.up * conePushForce, ForceMode.Impulse);
        }

        conesHitCount++;
        UpdateWarningCanvases();
    }

    private void UpdateWarningCanvases()
    {
        if (conesHitCount >= secondCanvasThreshold && !restarting)
        {
            restarting = true;
            ShowCanvas(secondWarningCanvas);
            StartCoroutine(RestartSceneAfterDelay());
        }
        else if (conesHitCount >= firstCanvasThreshold)
        {
            ShowCanvas(firstWarningCanvas);
        }
        else
        {
            HideCanvas(firstWarningCanvas);
        }
    }

    private void ShowCanvas(Canvas canvas)
    {
        if (canvas == firstWarningCanvas)
        {
            if (secondCanvasFade != null) StopCoroutine(secondCanvasFade);
            if (!canvas.enabled) canvas.enabled = true;
        }
        else if (canvas == secondWarningCanvas)
        {
            if (firstCanvasFade != null) StopCoroutine(firstCanvasFade);
            firstWarningCanvas.enabled = false;
            if (!canvas.enabled) canvas.enabled = true;
        }
    }

    private void HideCanvas(Canvas canvas)
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
        }
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetConesCount()
    {
        conesHitCount = 0;
        hitCones.Clear();
        HideCanvas(firstWarningCanvas);
        HideCanvas(secondWarningCanvas);
        restarting = false;
    }
}