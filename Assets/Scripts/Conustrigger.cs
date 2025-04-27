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
    public CanvasGroup firstWarningCanvas;
    public CanvasGroup secondWarningCanvas;

    private int conesHitCount;
    private HashSet<GameObject> hitCones = new HashSet<GameObject>();
    private bool restarting;

    private void Start()
    {
        SetCanvasAlpha(firstWarningCanvas, 0f);
        SetCanvasAlpha(secondWarningCanvas, 0f);
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
            StartCoroutine(FadeCanvas(secondWarningCanvas, 1f));
            StartCoroutine(RestartSceneAfterDelay());
        }
        else if (conesHitCount >= firstCanvasThreshold)
        {
            StartCoroutine(FadeCanvas(firstWarningCanvas, 1f));
        }
        else
        {
            StartCoroutine(FadeCanvas(firstWarningCanvas, 0f));
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, float targetAlpha)
    {
        float startAlpha = canvas.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        canvas.alpha = targetAlpha;
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetCanvasAlpha(CanvasGroup canvas, float alpha)
    {
        canvas.alpha = alpha;
    }

    public void ResetConesCount()
    {
        conesHitCount = 0;
        hitCones.Clear();
        StartCoroutine(FadeCanvas(firstWarningCanvas, 0f));
        StartCoroutine(FadeCanvas(secondWarningCanvas, 0f));
    }
}