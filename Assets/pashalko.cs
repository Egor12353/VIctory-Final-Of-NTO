using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip soundClip; // Перетащите сюда аудиофайл в инспекторе
    [SerializeField] private float volume = 1f;
    [SerializeField] private string targetTag = "Hand"; // Тег объекта, который должен активировать звук

    private AudioSource audioSource;

    private void Start()
    {
        // Инициализация AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем тег объекта, который вошел в триггер
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Объект с тегом 'рука' вошел в триггер");
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