using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    public Light directionalLight;
    public AudioClip firstSoundEffect;
    public AudioClip secondSoundEffect;

    private float[] cycleDurations = { 720f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f }; // Убран последний этап (3 секунды)
    private int currentStep = 0;
    private float elapsedTime = 0f;
    private AudioSource audioSource;
    private bool firstSoundPlayed = false;
    private bool secondSoundPlayed = false;

    // Дополнительные переменные для плавного выключения второго звука
    private bool isFadingOut = false;
    private float fadeOutTimer = 0f;
    private const float fadeOutDuration = 2f; // Продолжительность выключения (в секундах)
    private const float secondSoundDuration = 20f; // Время до начала выключения

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;

        // Устанавливаем начальное положение света на 90 градусов по оси X
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        ResetCycle();
    }

    void Update()
    {
        HandleInput();
        UpdateCycle();
        RotateLight();
        CheckSoundTriggers();

        // Логика плавного выключения второго звука
        if (isFadingOut)
        {
            fadeOutTimer += Time.deltaTime;

            // Вычисляем новую громкость
            float newVolume = Mathf.Lerp(1f, 0f, fadeOutTimer / fadeOutDuration);
            audioSource.volume = newVolume;

            // Если выключение завершено, останавливаем звук
            if (fadeOutTimer >= fadeOutDuration)
            {
                audioSource.Stop();
                isFadingOut = false;
                fadeOutTimer = 0f;
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Переход к следующему этапу цикла
            currentStep = Mathf.Min(currentStep + 1, cycleDurations.Length - 1);
            ResetCycle();
            CheckSoundForCurrentStep(); // Проверка звука для текущего этапа
        }
    }

    void UpdateCycle()
    {
        elapsedTime += Time.deltaTime;

        // Если время текущего этапа истекло
        if (elapsedTime >= cycleDurations[currentStep])
        {
            if (currentStep == cycleDurations.Length - 1)
            {
                // Последний этап: устанавливаем свет на 90 градусов и включаем второй звук
                if (directionalLight != null)
                {
                    directionalLight.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                }

                PlaySecondSound();
                ResetToTwelveMinutes();
            }
            else
            {
                // Переход к следующему этапу
                currentStep++;
                ResetCycle();
                CheckSoundForCurrentStep(); // Проверка звука для нового этапа
            }
        }
    }

    void RotateLight()
    {
        if (directionalLight != null && currentStep < cycleDurations.Length - 1) // Только если это не последний этап
        {
            float progress = elapsedTime / cycleDurations[currentStep];

            // Разделяем движение на два этапа: от 90 до 360 и от 0 до 90
            float angle;
            if (progress <= 0.5f)
            {
                // Первый этап: от 90 до 360
                angle = Mathf.Lerp(90f, 360f, progress * 2f);
            }
            else
            {
                // Второй этап: от 0 до 90
                angle = Mathf.Lerp(0f, 90f, (progress - 0.5f) * 2f);
            }

            directionalLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);
        }
    }

    void CheckSoundTriggers()
    {
        // Первый звук играет при переходе к 5-секундному циклу
        if (currentStep == 1 && !firstSoundPlayed)
        {
            PlaySound(firstSoundEffect);
            firstSoundPlayed = true;
        }
    }

    void CheckSoundForCurrentStep()
    {
        // Логика проверки звуков для текущего этапа
        if (currentStep == 1 && !firstSoundPlayed)
        {
            PlaySound(firstSoundEffect);
            firstSoundPlayed = true;
        }
        else if (currentStep == cycleDurations.Length - 1 && !secondSoundPlayed)
        {
            PlaySecondSound();
        }
    }

    void PlaySecondSound()
    {
        if (!secondSoundPlayed)
        {
            PlaySound(secondSoundEffect);
            secondSoundPlayed = true;

            // Запускаем таймер для выключения звука
            StartCoroutine(FadeOutSecondSound());
        }
    }

    System.Collections.IEnumerator FadeOutSecondSound()
    {
        yield return new WaitForSeconds(secondSoundDuration); // Ждем 20 секунд
        isFadingOut = true; // Начинаем плавное выключение
    }

    void ResetToTwelveMinutes()
    {
        currentStep = 0;
        ResetCycle();
        firstSoundPlayed = false;
        secondSoundPlayed = false;

        // Устанавливаем свет обратно на 90 градусов по оси X
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }

    void ResetCycle()
    {
        elapsedTime = 0f;
        Debug.Log($"Cycle set to: {cycleDurations[currentStep]} seconds");
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.volume = 1f; // Устанавливаем начальную громкость
            audioSource.Play();
        }
    }
}