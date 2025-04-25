using UnityEngine;
using System.Collections;

public class CarEngineController : MonoBehaviour
{
    [Header("Engine Sounds")]
    public AudioClip ignitionSound;
    public AudioSource audioSource;
    [Range(0, 1)] public float successRate = 0.5f;

    [Header("References")]
    public CarController carController;
    public EnterInCar enterInCar;

    [Header("Engine State")]
    public bool isEngineStarted = false;
    private bool isIgnitionPlaying = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        if (enterInCar == null)
        {
            enterInCar = GetComponent<EnterInCar>();
        }
    }

    void Update()
    {
        // ѕровер€ем, находитс€ ли игрок в этом транспортном средстве
        if (!enterInCar.inDrive) return;

        if (enterInCar.isInTruck)
        {
            HandleTruckInput();
        }
        else
        {
            HandleBusInput();
        }
    }

    void HandleTruckInput()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isEngineStarted && !isIgnitionPlaying)
        {
            StartCoroutine(AttemptStartEngine());
        }

        if (Input.GetKeyDown(KeyCode.N) && isEngineStarted)
        {
            StopEngine();
        }
    }

    void HandleBusInput()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isEngineStarted)
        {
            StartCoroutine(StartBusEngine());
        }

        if (Input.GetKeyDown(KeyCode.N) && isEngineStarted)
        {
            StopEngine();
        }
    }

    IEnumerator AttemptStartEngine()
    {
        isIgnitionPlaying = true;
        PlaySound(ignitionSound);

        yield return new WaitForSeconds(ignitionSound.length - 10f);

        if (Random.value <= successRate)
        {
            StartEngineRunning();
        }
        else
        {
            print("√рузовик не завелс€!");
            isIgnitionPlaying = false;
        }
    }

    IEnumerator StartBusEngine()
    {
        isIgnitionPlaying = true;
        PlaySound(ignitionSound);

        yield return new WaitForSeconds(ignitionSound.length - 5f);

        StartEngineRunning();
    }

    void StartEngineRunning()
    {
        print("sound");
        isEngineStarted = true;
        isIgnitionPlaying = false;

        if (carController != null)
        {
            carController.OnEngineStarted();
        }
    }

    void StopEngine()
    {
        isEngineStarted = false;
        audioSource.Stop();

        if (carController != null)
        {
            carController.OnEngineStopped();
        }
    }

    void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
}