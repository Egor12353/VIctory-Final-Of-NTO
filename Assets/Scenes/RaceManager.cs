using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    [Header("Triggers Settings")]
    [SerializeField] private Collider[] raceTriggers;
    [SerializeField] private Collider boundaryTrigger;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 3f;

    [Header("UI Settings")]
    [SerializeField] private Canvas wrongWayCanvas;
    [SerializeField] private Canvas outOfBoundsCanvas;
    [SerializeField] private Canvas exitVehicleCanvas;
    [Header("Settings")]
    [SerializeField] private int wrongWayThreshold = 2; // Добавьте эту строку

    private int currentTriggerIndex = -1;
    private int wrongDirectionCount = 0;
    private bool isInBounds = true;
    private bool isInVehicle = true;

    private void Start()
    {
        InitializeUI();
        SetupFade();
    }

    private void InitializeUI()
    {
        wrongWayCanvas.enabled = false;
        outOfBoundsCanvas.enabled = false;
        exitVehicleCanvas.enabled = false;
    }

    private void SetupFade()
    {
        if (fadeImage != null)
        {
            fadeImage.color = Color.clear;
            fadeImage.raycastTarget = false;
            fadeImage.gameObject.SetActive(false);
        }
    }

    public void ReportTrigger(Collider trigger)
    {
        int triggerIndex = System.Array.IndexOf(raceTriggers, trigger);

        if (triggerIndex == currentTriggerIndex + 1)
        {
            currentTriggerIndex = triggerIndex;
            wrongDirectionCount = 0;
        }
        else if (triggerIndex < currentTriggerIndex)
        {
            wrongDirectionCount++;
            if (wrongDirectionCount >= wrongWayThreshold)
            {
                ShowWrongWayCanvas();
            }
        }

        if (currentTriggerIndex == raceTriggers.Length - 1)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeImage.color = Color.Lerp(Color.clear, Color.black, timer / fadeDuration);
                yield return null;
            }
        }

        SceneManager.LoadScene("FinalScene");
    }

    // Остальные методы остаются без изменений
    private void OnTriggerExit(Collider other)
    {
        if (other == boundaryTrigger)
        {
            isInBounds = false;
            outOfBoundsCanvas.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == boundaryTrigger)
        {
            isInBounds = true;
            outOfBoundsCanvas.enabled = false;
        }

        if (other.CompareTag("Player") && isInVehicle)
        {
            isInVehicle = false;
            exitVehicleCanvas.enabled = true;
        }
    }

    private void ShowWrongWayCanvas()
    {
        wrongWayCanvas.enabled = true;
        StartCoroutine(HideCanvasAfterDelay(wrongWayCanvas, 3f));
    }

    private IEnumerator HideCanvasAfterDelay(Canvas canvas, float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.enabled = false;
    }

    public void ReenterVehicle()
    {
        isInVehicle = true;
        exitVehicleCanvas.enabled = false;
    }
}