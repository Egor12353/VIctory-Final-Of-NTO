using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class CarEngineController : MonoBehaviour
{
    [Header("Engine Sounds")]
    public AudioClip ignitionSound;
    public AudioSource audioSource;
    [Range(0, 1)] public float successRate = 0.5f;
    [SerializeField]
    public SteamVR_Action_Boolean carEng;
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
        if (!enterInCar.inDrive) return;

        if (enterInCar.isInTruck && enterInCar.inDrive == true)
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
        if (carEng.GetStateDown(SteamVR_Input_Sources.RightHand)) // R1
        {
            if (!isEngineStarted && !isIgnitionPlaying)
            {
                StartCoroutine(AttemptStartEngine());
            }
            else if (isEngineStarted)
            {
                StopEngine();
            }
        }
    }

    void HandleBusInput()
    {
        if (carEng.GetStateDown(SteamVR_Input_Sources.RightHand)) // R1
        {
            if (!isEngineStarted && !isIgnitionPlaying)
            {
                StartCoroutine(StartBusEngine());
            }
            else if (isEngineStarted)
            {
                StopEngine();
            }
        }
    }

    IEnumerator AttemptStartEngine()
    {
        isIgnitionPlaying = true;
        PlaySound(ignitionSound);

        yield return new WaitForSeconds(ignitionSound.length - 15f);

        if (Random.value <= successRate)
        {
            StartEngineRunning();
        }
        else
        {
            print("Engine Failed to Start!");
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
        print("Engine Started");
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