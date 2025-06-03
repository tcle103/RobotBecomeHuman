/*Last modified by: Dale Spence
 * Last modified on: 5 / 15 / 25
 *
 * Pause Menu Controller handles navigating and managing the pause menu. 
*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Control")]
    public CanvasGroup pauseCanvasGroup;
    public GameObject firstSelected;
    public AudioSource musicSource;

    public bool isPaused = false;

    [Header("Menu Navigation")]
    public List<TextMeshProUGUI> menuOptions; // Load, Options, Menu, Quit
    private int selectedIndex = 0;
    private string arrow = ">";
    private string padding = "  ";

    [Header("Options Navigation")]
    public CanvasGroup optionsPanelGroup;
    public List<TextMeshProUGUI> optionsMenuItems; // Music, SFX, Return
    private int optionsSelectedIndex = 0;
    private bool inOptionsMenu = false;

    void Update()
    {
        // Toggle pause
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (!isPaused) return;

        if (inOptionsMenu)
        {
            HandleOptionsMenuInput();
        }
        else
        {
            HandlePauseMenuInput();
        }
    }

    void HandlePauseMenuInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateSelectionVisuals();
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            selectedIndex = Mathf.Min(menuOptions.Count - 1, selectedIndex + 1);
            UpdateSelectionVisuals();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
        {
            ActivateSelection();
        }
    }

    void HandleOptionsMenuInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            optionsSelectedIndex = Mathf.Max(0, optionsSelectedIndex - 1);
            UpdateOptionsVisuals();
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            optionsSelectedIndex = Mathf.Min(optionsMenuItems.Count - 1, optionsSelectedIndex + 1);
            UpdateOptionsVisuals();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (optionsSelectedIndex == optionsMenuItems.Count - 1) // Last item = "Return"
            {
                ShowPausePanel();
            }
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (musicSource != null)
        {
            if (isPaused) musicSource.Pause();
            else musicSource.UnPause();
        }

        if (isPaused)
        {
            ShowPausePanel();
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
        else
        {
            pauseCanvasGroup.alpha = 0;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;

            optionsPanelGroup.alpha = 0;
            optionsPanelGroup.interactable = false;
            optionsPanelGroup.blocksRaycasts = false;

            EventSystem.current.SetSelectedGameObject(null);
        }

        // Tell the inventory it shouldn't be interactable
        var inventory = FindObjectOfType<InventoryMenuController>();
        if (inventory != null)
        {
            inventory.gameIsPaused = isPaused;
            if (isPaused) inventory.ForceCloseInventory();
        }
    }

    void ActivateSelection()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Load selected");
                // find the save system and load the game
                var saveSystem = FindObjectOfType<SaveSystem>();
                if (saveSystem != null)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    saveSystem.gameLoad();
                    Debug.Log("Game loaded successfully.");
                }
                else
                {
                    Debug.LogError("SaveSystem not found!");
                }
                break;
            case 1:
                Debug.Log("Options selected");
                ShowOptionsPanel();
                break;
            case 2:
                Debug.Log("Menu selected");
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
                break;
            case 3:
                Debug.Log("Quit selected");
                Application.Quit();
                break;
        }
    }

    void ShowOptionsPanel()
    {
        pauseCanvasGroup.alpha = 0;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;

        optionsPanelGroup.gameObject.SetActive(true); // Make sure it's visible


        optionsPanelGroup.alpha = 1;
        optionsPanelGroup.interactable = true;
        optionsPanelGroup.blocksRaycasts = true;

        inOptionsMenu = true;
        optionsSelectedIndex = 0;
        UpdateOptionsVisuals();

        Debug.Log("Options Panel Found! Setting visible");

    }

    void ShowPausePanel()
    {
        optionsPanelGroup.alpha = 0;
        optionsPanelGroup.interactable = false;
        optionsPanelGroup.blocksRaycasts = false;

        optionsPanelGroup.gameObject.SetActive(false); // Hide it completely


        pauseCanvasGroup.alpha = 1;
        pauseCanvasGroup.interactable = true;
        pauseCanvasGroup.blocksRaycasts = true;

        inOptionsMenu = false;
        selectedIndex = 0;
        UpdateSelectionVisuals();
    }

    void UpdateSelectionVisuals()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {
            string label = menuOptions[i].text.TrimStart('>', ' ');
            menuOptions[i].text = (i == selectedIndex ? arrow : padding) + label;
        }
    }

    void UpdateOptionsVisuals()
    {
        for (int i = 0; i < optionsMenuItems.Count; i++)
        {
            string label = optionsMenuItems[i].text.TrimStart('>', ' ');
            optionsMenuItems[i].text = (i == optionsSelectedIndex ? arrow : padding) + label;
        }
    }
}