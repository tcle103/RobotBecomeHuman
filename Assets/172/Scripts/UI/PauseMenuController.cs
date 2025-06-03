/*Last modified by: Dale Spence
 * Last modified on: 5 / 27 / 25
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
    public List<TextMeshProUGUI> optionsMenuItems; // Option1, Option2, Return
    private int optionsSelectedIndex = 0;
    private bool inOptionsMenu = false;

    [Header("FPS Settings Panel")]
    public CanvasGroup fpsSettingsPanelGroup;
    public List<TextMeshProUGUI> fpsLimitItems;   // 30, 60, 120, 240, Off
    public List<TextMeshProUGUI> fpsDisplayItems; // Show, Hide, Return
    private int fpsLimitIndex = 0;
    private int fpsDisplayIndex = 0;
    private bool inFPSOptionsMenu = false;
    private bool editingLimitGroup = true;

    public FPSController fpsController;
    public FPSCounter fpsCounter;

    void Start()
    {
        HideAllPanels();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (!isPaused) return;

        if (inFPSOptionsMenu)
        {
            HandleFPSOptionsInput();
        }
        else if (inOptionsMenu)
        {
            HandleOptionsMenuInput();
        }
        else
        {
            HandlePauseMenuInput();
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
            HideAllPanels();
            EventSystem.current.SetSelectedGameObject(null);
        }

        var inventory = FindObjectOfType<InventoryMenuController>();
        if (inventory != null)
        {
            inventory.gameIsPaused = isPaused;
            if (isPaused) inventory.ForceCloseInventory();
        }
    }

    void HandlePauseMenuInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateSelectionVisuals(menuOptions, selectedIndex);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            selectedIndex = Mathf.Min(menuOptions.Count - 1, selectedIndex + 1);
            UpdateSelectionVisuals(menuOptions, selectedIndex);
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
            UpdateSelectionVisuals(optionsMenuItems, optionsSelectedIndex);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            optionsSelectedIndex = Mathf.Min(optionsMenuItems.Count - 1, optionsSelectedIndex + 1);
            UpdateSelectionVisuals(optionsMenuItems, optionsSelectedIndex);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (optionsSelectedIndex == 0) // Option1 = FPS Settings
            {
                ShowFPSOptionsPanel();
            }
            else if (optionsSelectedIndex == optionsMenuItems.Count - 1) // Return
            {
                ShowPausePanel();
            }
        }
    }

    void HandleFPSOptionsInput()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
            editingLimitGroup = true;
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
            editingLimitGroup = false;

        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (editingLimitGroup)
                fpsLimitIndex = Mathf.Max(0, fpsLimitIndex - 1);
            else
                fpsDisplayIndex = Mathf.Max(0, fpsDisplayIndex - 1);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            if (editingLimitGroup)
                fpsLimitIndex = Mathf.Min(fpsLimitItems.Count - 1, fpsLimitIndex + 1);
            else
                fpsDisplayIndex = Mathf.Min(fpsDisplayItems.Count - 1, fpsDisplayIndex + 1);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (editingLimitGroup)
            {
                fpsController.SetFPSLimit((FPSController.FPSLimit)fpsLimitIndex);
            }
            else
            {
                if (fpsDisplayIndex == 0)
                {
                    fpsCounter.showFPS = true;
                    if (fpsCounter.fpsText != null)
                        fpsCounter.fpsText.gameObject.SetActive(true);
                }
                else if (fpsDisplayIndex == 1)
                {
                    fpsCounter.showFPS = false;
                    if (fpsCounter.fpsText != null)
                        fpsCounter.fpsText.gameObject.SetActive(false);
                }
                else if (fpsDisplayIndex == 2)
                {
                    ShowOptionsPanel();
                }
            }
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ShowOptionsPanel();
        }

        UpdateFPSOptionsVisuals();
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

    void ShowPausePanel()
    {
        HideAllPanels();

        pauseCanvasGroup.alpha = 1;
        pauseCanvasGroup.interactable = true;
        pauseCanvasGroup.blocksRaycasts = true;

        inOptionsMenu = false;
        inFPSOptionsMenu = false;
        selectedIndex = 0;
        UpdateSelectionVisuals(menuOptions, selectedIndex);
    }

    void ShowOptionsPanel()
    {
        HideAllPanels();

        optionsPanelGroup.gameObject.SetActive(true);
        optionsPanelGroup.alpha = 1;
        optionsPanelGroup.interactable = true;
        optionsPanelGroup.blocksRaycasts = true;

        inOptionsMenu = true;
        inFPSOptionsMenu = false;
        optionsSelectedIndex = 0;
        UpdateSelectionVisuals(optionsMenuItems, optionsSelectedIndex);
    }

    void ShowFPSOptionsPanel()
    {
        HideAllPanels();

        fpsSettingsPanelGroup.gameObject.SetActive(true);
        fpsSettingsPanelGroup.alpha = 1;
        fpsSettingsPanelGroup.interactable = true;
        fpsSettingsPanelGroup.blocksRaycasts = true;

        inFPSOptionsMenu = true;
        inOptionsMenu = false;
        fpsLimitIndex = 0;
        fpsDisplayIndex = 0;
        editingLimitGroup = true;

        UpdateFPSOptionsVisuals();
    }

    void HideAllPanels()
    {
        pauseCanvasGroup.alpha = 0;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;

        optionsPanelGroup.alpha = 0;
        optionsPanelGroup.interactable = false;
        optionsPanelGroup.blocksRaycasts = false;
        optionsPanelGroup.gameObject.SetActive(false);

        fpsSettingsPanelGroup.alpha = 0;
        fpsSettingsPanelGroup.interactable = false;
        fpsSettingsPanelGroup.blocksRaycasts = false;
        fpsSettingsPanelGroup.gameObject.SetActive(false);
    }

    void UpdateSelectionVisuals(List<TextMeshProUGUI> items, int selected)
    {
        for (int i = 0; i < items.Count; i++)
        {
            string label = items[i].text.TrimStart('>', ' ');
            items[i].text = (i == selected ? arrow : padding) + label;
        }
    }

    void UpdateFPSOptionsVisuals()
    {
        for (int i = 0; i < fpsLimitItems.Count; i++)
        {
            string label = fpsLimitItems[i].text.Replace("[x]", "").Replace("[ ]", "").Replace(">", "").Trim();
            string checkbox = (fpsController.currentFPSLimit == (FPSController.FPSLimit)i) ? "[x]" : "[ ]";
            string selector = (editingLimitGroup && i == fpsLimitIndex) ? "> " : "  ";
            fpsLimitItems[i].text = selector + checkbox + " " + label;
        }

        for (int i = 0; i < fpsDisplayItems.Count; i++)
        {
            string label = fpsDisplayItems[i].text.Replace("[x]", "").Replace("[ ]", "").Replace(">", "").Trim();
            string checkbox = "[ ]";

            if (i == 0 && fpsCounter.showFPS) checkbox = "[x]";
            else if (i == 1 && !fpsCounter.showFPS) checkbox = "[x]";

            string selector = (!editingLimitGroup && i == fpsDisplayIndex) ? "> " : "  ";
            fpsDisplayItems[i].text = selector + checkbox + " " + label;
        }
    }
}
