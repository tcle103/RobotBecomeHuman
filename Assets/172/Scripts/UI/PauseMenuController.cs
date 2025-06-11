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
using UnityEngine.Localization.Settings;
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
    
    public CanvasGroup colorBlindSettingsPanelGroup;
    public List<TextMeshProUGUI> colorBlindOptionsItems; // Return, Normal, Protanopia, Deuteranopia, Tritanopia
    private int colorBlindOptionsIndex = 0;
    private bool inColorBlindOptionsMenu = false;
    
    public CanvasGroup languageSettingsPanelGroup;
    public List<TextMeshProUGUI> languageOptionsItems; // Return, English, Japanese, Chinese
    private int languageOptionsIndex = 0;
    private bool inLanguageOptionsMenu = false;

    public FPSController fpsController;
    public FPSCounter fpsCounter;
    
    public SaveSystem saveSystem;

    public PlayerController movementScript;

    void Start()
    {
        HideAllPanels();
        saveSystem = FindObjectOfType<SaveSystem>();
        movementScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || 
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
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
        }else if (inColorBlindOptionsMenu)
        {
            HandleColorBlindOptionsInput();
        }
        else if (inLanguageOptionsMenu)
        {
            HandleLanguageOptionsInput();
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

        if (movementScript == null)
        {
            movementScript = FindObjectOfType<PlayerController>();
            
        }
        movementScript.enabled = !isPaused;

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
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || 
            (Gamepad.current != null && (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame)))
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateSelectionVisuals(menuOptions, selectedIndex);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 (Gamepad.current != null && (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame)))
        {
            selectedIndex = Mathf.Min(menuOptions.Count - 1, selectedIndex + 1);
            UpdateSelectionVisuals(menuOptions, selectedIndex);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame ||
            (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            ActivateSelection();
        }
    }

    void HandleOptionsMenuInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || 
                (Gamepad.current != null && (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame)))
        {
            optionsSelectedIndex = Mathf.Max(0, optionsSelectedIndex - 1);
            UpdateSelectionVisuals(optionsMenuItems, optionsSelectedIndex);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 (Gamepad.current != null && (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame)))
        {
            optionsSelectedIndex = Mathf.Min(optionsMenuItems.Count - 1, optionsSelectedIndex + 1);
            UpdateSelectionVisuals(optionsMenuItems, optionsSelectedIndex);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame ||
                (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            if (optionsSelectedIndex == 0) // Return
            {
                ShowPausePanel();
            }else if (optionsSelectedIndex == 1) // Option1 = FPS Settings
            {
                ShowFPSOptionsPanel();
            }
            else if (optionsSelectedIndex == 2) // Option 2 = Colorblind Settings
            {
                ShowColorBlindOptionsPanel();
            }else if (optionsSelectedIndex == 3) // Option 3 = Language Settings
            {
                ShowLanguageOptionsPanel();
            }
        }
    }

    void HandleFPSOptionsInput()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame || 
            (Gamepad.current != null && (Gamepad.current.dpad.left.wasPressedThisFrame || Gamepad.current.leftStick.left.wasPressedThisFrame)))
            editingLimitGroup = true;
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame ||
                 (Gamepad.current != null && (Gamepad.current.dpad.right.wasPressedThisFrame || Gamepad.current.leftStick.right.wasPressedThisFrame)))
            editingLimitGroup = false;

        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || 
                (Gamepad.current != null && (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame)))
        {
            if (editingLimitGroup)
                fpsLimitIndex = Mathf.Max(0, fpsLimitIndex - 1);
            else
                fpsDisplayIndex = Mathf.Max(0, fpsDisplayIndex - 1);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 (Gamepad.current != null && (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame)))
        {
            if (editingLimitGroup)
                fpsLimitIndex = Mathf.Min(fpsLimitItems.Count - 1, fpsLimitIndex + 1);
            else
                fpsDisplayIndex = Mathf.Min(fpsDisplayItems.Count - 1, fpsDisplayIndex + 1);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame ||
            (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            if (editingLimitGroup)
            {
                fpsController.SetFPSLimit((FPSController.FPSLimit)fpsLimitIndex);
            }
            else
            {
                if (fpsDisplayIndex == 0)
                {
                    ShowOptionsPanel();
                }
                else if (fpsDisplayIndex == 1)
                {
                    fpsCounter.showFPS = true;
                    if (fpsCounter.fpsText != null)
                        fpsCounter.fpsText.gameObject.SetActive(true);
                }
                else if (fpsDisplayIndex == 2)
                {
                    fpsCounter.showFPS = false;
                    if (fpsCounter.fpsText != null)
                        fpsCounter.fpsText.gameObject.SetActive(false);
                }
            }
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame || 
            (Gamepad.current != null && (Gamepad.current.selectButton.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame)))
        {
            ShowOptionsPanel();
        }

        UpdateFPSOptionsVisuals();
    }

    void HandleColorBlindOptionsInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame ||
            (Gamepad.current != null && (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame)))
        {
            colorBlindOptionsIndex = Mathf.Max(0, colorBlindOptionsIndex - 1);
            UpdateSelectionVisuals(colorBlindOptionsItems, colorBlindOptionsIndex);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 (Gamepad.current != null && (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame)))
        {
            colorBlindOptionsIndex = Mathf.Min(colorBlindOptionsItems.Count - 1, colorBlindOptionsIndex + 1);
            UpdateSelectionVisuals(colorBlindOptionsItems, colorBlindOptionsIndex);
        }
        
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame ||
            (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            switch (colorBlindOptionsIndex)
            {
                case 0: // Normal
                    ShowOptionsPanel();
                    return;
                case 1: // Protanopia
                    saveSystem.SetColorblindType(0);
                    break;
                case 2: // Protanopia
                    saveSystem.SetColorblindType(1);
                    break;
                case 3: // Deuteranopia
                    saveSystem.SetColorblindType(2);
                    break;
                case 4: // Tritanopia
                    saveSystem.SetColorblindType(3);
                    break;
            }
        }
    }
    
    void HandleLanguageOptionsInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame ||
            (Gamepad.current != null && (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame)))
        {
            languageOptionsIndex = Mathf.Max(0, languageOptionsIndex - 1);
            UpdateSelectionVisuals(languageOptionsItems, languageOptionsIndex);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 (Gamepad.current != null && (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame)))
        {
            languageOptionsIndex = Mathf.Min(languageOptionsItems.Count - 1, languageOptionsIndex + 1);
            UpdateSelectionVisuals(languageOptionsItems, languageOptionsIndex);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame ||
            (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame)))
        {
            switch (languageOptionsIndex)
            {
                case 0: // Return
                    ShowOptionsPanel();
                    return;
                case 1: // English
                    //change locale to English
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                    break;
                case 2: // Japanese
                    //change locale to Japanese
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                    break;
                case 3: // Chinese
                    //change locale to Chinese
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
                    break;
            }
        }
    }

    void ActivateSelection()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Load selected");
                TogglePause();
                //// find the save system and load the game
                //var saveSystem = FindObjectOfType<SaveSystem>();
                //if (saveSystem != null)
                //{
                //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //    saveSystem.gameLoad();
                //    Debug.Log("Game loaded successfully.");
                //}
                //else
                //{
                //    Debug.LogError("SaveSystem not found!");
                //}
                break;
            case 1:
                Debug.Log("Options selected");
                ShowOptionsPanel();
                break;
            case 2:
                Debug.Log("Menu selected");
                Time.timeScale = 1f;
                saveSystem.analyticsManager.Restart();
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
        inColorBlindOptionsMenu = false;
        inLanguageOptionsMenu = false;
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
        inColorBlindOptionsMenu = false;
        inLanguageOptionsMenu = false;
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
        inColorBlindOptionsMenu = false;
        inLanguageOptionsMenu = false;
        fpsLimitIndex = 0;
        fpsDisplayIndex = 0;
        editingLimitGroup = true;

        UpdateFPSOptionsVisuals();
    }
    
    void ShowColorBlindOptionsPanel()
    {
        HideAllPanels();

        colorBlindSettingsPanelGroup.gameObject.SetActive(true);
        colorBlindSettingsPanelGroup.alpha = 1;
        colorBlindSettingsPanelGroup.interactable = true;
        colorBlindSettingsPanelGroup.blocksRaycasts = true;

        inColorBlindOptionsMenu = true;
        inOptionsMenu = false;
        inFPSOptionsMenu = false;
        inLanguageOptionsMenu = false;
        colorBlindOptionsIndex = 0;
        UpdateSelectionVisuals(colorBlindOptionsItems, colorBlindOptionsIndex);
    }
    
    void ShowLanguageOptionsPanel()
    {
        HideAllPanels();

        languageSettingsPanelGroup.gameObject.SetActive(true);
        languageSettingsPanelGroup.alpha = 1;
        languageSettingsPanelGroup.interactable = true;
        languageSettingsPanelGroup.blocksRaycasts = true;

        inLanguageOptionsMenu = true;
        inOptionsMenu = false;
        inFPSOptionsMenu = false;
        inColorBlindOptionsMenu = false;
        languageOptionsIndex = 0;
        UpdateSelectionVisuals(languageOptionsItems, languageOptionsIndex);
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
        
        colorBlindSettingsPanelGroup.alpha = 0;
        colorBlindSettingsPanelGroup.interactable = false;
        colorBlindSettingsPanelGroup.blocksRaycasts = false;
        colorBlindSettingsPanelGroup.gameObject.SetActive(false);
        
        languageSettingsPanelGroup.alpha = 0;
        languageSettingsPanelGroup.interactable = false;
        languageSettingsPanelGroup.blocksRaycasts = false;
        languageSettingsPanelGroup.gameObject.SetActive(false);
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
