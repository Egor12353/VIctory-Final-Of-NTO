using UnityEngine;

public class pashalko : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip soundClip; // Перетащите сюда аудиофайл в инспекторе
    [SerializeField] private float volume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        // Инициализация AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void Update()
    {
        // Проверяем нажатие клавиши M
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (soundClip != null)
        {
            // Проигрываем звук один раз без прерывания предыдущего воспроизведения
            audioSource.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("Audio clip не назначен!");
        }
    }

    // Метод для изменения громкости (опционально)
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }
}