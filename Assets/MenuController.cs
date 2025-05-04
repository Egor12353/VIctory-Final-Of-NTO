using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.H;
    [SerializeField] private bool pauseGame = true;

    private bool isMenuVisible = false;

    private void Awake()
    {
        ValidateReferences();
        InitializeMenu();
    }

    private void ValidateReferences()
    {
        if (menuPanel == null)
        {
            Debug.LogError("Menu Panel is not assigned in the inspector!", this);
            enabled = false; // Отключаем скрипт при отсутствии ссылки
        }
    }

    private void InitializeMenu()
    {
        menuPanel.SetActive(false);
        isMenuVisible = false;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            print(isMenuVisible);
            Debug.Log($"Toggling menu with key: {toggleKey}");
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        UpdateMenuState();
    }

    private void UpdateMenuState()
    {
        if (menuPanel == null) return;

        menuPanel.SetActive(isMenuVisible);

        if (pauseGame)
        {
            Time.timeScale = isMenuVisible ? 0f : 1f;
            Debug.Log($"Game paused: {isMenuVisible}");
        }
    }

    // Для кнопки закрытия меню (если нужно)
    public void CloseMenu()
    {
        isMenuVisible = false;
        UpdateMenuState();
    }
}