using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VRbutton : MonoBehaviour
{
    public enum ButtonAction { Restart, ToggleMusic, ExitScene }

    [Header("Settings")]
    [SerializeField] private ButtonAction actionType;
    [SerializeField] private string mainMenuName = "MainMenu";

    private Button button;
    private AudioSource musicSource;

    private void Awake()
    {
        button = GetComponent<Button>();
        musicSource = GameObject.FindGameObjectWithTag("MusicPlayer")?.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            button.onClick.Invoke();
            ChangeButtonColor(new Color(0.7f, 0.7f, 0.7f));
            ExecuteAction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            ChangeButtonColor(Color.white);
        }
    }

    private void ChangeButtonColor(Color color)
    {
        if (button.TryGetComponent<Image>(out var image))
        {
            image.color = color;
        }
    }

    private void ExecuteAction()
    {
        switch (actionType)
        {
            case ButtonAction.Restart:
                RestartGame();
                break;

            case ButtonAction.ToggleMusic:
                ToggleMusic();
                break;

            case ButtonAction.ExitScene:
                ExitToMainMenu();
                break;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute;
        }
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}