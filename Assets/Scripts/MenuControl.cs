using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    
    public static SettingsSave settingsSave;
    
    private String[] languages = {"English", "Japanese", "Chinese"};
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject languageButton;
    [SerializeField] private GameObject contrastButton;
    private Dictionary<String, Sprite> menuSprites;
    // tien things
    private CanvasGroup canvasgroup;

    // Start is called before the first frame update
    void Start()
    {
        settingsSave = FindObjectOfType<SettingsSave>();
        settingsSave.npcs.Clear();
        menuSprites = new Dictionary<String, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Menus");
        foreach (Sprite sprite in sprites)
        {
            menuSprites.Add(sprite.name, sprite);
        }

        // tien thing again
        canvasgroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        //load language and contrast from Menu options file
        string language = settingsSave.language;
        string contrast = settingsSave.contrast;
        
        
        //if scene is menu - this code is ugly in a rush
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            if (language == "English")
            {
                logo.GetComponent<Image>().sprite = menuSprites["ENGlogo"];
                controls.GetComponent<Image>().sprite = menuSprites["ENGcontrols"];
                startButton.GetComponent<Image>().sprite = menuSprites["ENGstart"];
                optionsButton.GetComponent<Image>().sprite = menuSprites["ENGsettings"];
                quitButton.GetComponent<Image>().sprite = menuSprites["ENGquit"];
            }
            else if (language == "Japanese")
            {
                logo.GetComponent<Image>().sprite = menuSprites["JPNlogo"];
                controls.GetComponent<Image>().sprite = menuSprites["JPNcontrols"];
                startButton.GetComponent<Image>().sprite = menuSprites["JPNstart"];
                optionsButton.GetComponent<Image>().sprite = menuSprites["JPNsettings"];
                quitButton.GetComponent<Image>().sprite = menuSprites["JPNquit"];
            }
            else if (language == "Chinese")
            {
                logo.GetComponent<Image>().sprite = menuSprites["CHNlogo"];
                controls.GetComponent<Image>().sprite = menuSprites["CHNcontrols"];
                startButton.GetComponent<Image>().sprite = menuSprites["CHNstart"];
                optionsButton.GetComponent<Image>().sprite = menuSprites["CHNsettings"];
                quitButton.GetComponent<Image>().sprite = menuSprites["CHNquit"];
            }
        }else if (SceneManager.GetActiveScene().name == "OptionMenu")
        {
            if (language == "English")
            {
                backButton.GetComponent<Image>().sprite = menuSprites["ENGback"];
                languageButton.GetComponent<Image>().sprite = menuSprites["ENGlang"];
                contrastButton.GetComponent<Image>().sprite = menuSprites["ENGcontrast"];
            }
            else if (language == "Japanese")
            {
                backButton.GetComponent<Image>().sprite = menuSprites["JPNback"];
                languageButton.GetComponent<Image>().sprite = menuSprites["JPNlang"];
                contrastButton.GetComponent<Image>().sprite = menuSprites["JPNcontrast"];
            }
            else if (language == "Chinese")
            {
                backButton.GetComponent<Image>().sprite = menuSprites["CHNback"];
                languageButton.GetComponent<Image>().sprite = menuSprites["CHNlang"];
                contrastButton.GetComponent<Image>().sprite = menuSprites["CHNcontrast"];
            }
        }
        else if (this.name == "OptionsCanvas")
        {
            if (language == "English")
            {
                backButton.GetComponent<Image>().sprite = menuSprites["ENGback"];
                languageButton.GetComponent<Image>().sprite = menuSprites["ENGlang"];
                contrastButton.GetComponent<Image>().sprite = menuSprites["ENGcontrast"];
            }
            else if (language == "Japanese")
            {
                backButton.GetComponent<Image>().sprite = menuSprites["JPNback"];
                languageButton.GetComponent<Image>().sprite = menuSprites["JPNlang"];
                contrastButton.GetComponent<Image>().sprite = menuSprites["JPNcontrast"];
            }
            else if (language == "Chinese")
            {
                backButton.GetComponent<Image>().sprite = menuSprites["CHNback"];
                languageButton.GetComponent<Image>().sprite = menuSprites["CHNlang"];
                contrastButton.GetComponent<Image>().sprite = menuSprites["CHNcontrast"];
            }
        }
        else if (this.name == "UICanvas")
        {
            if (language == "English")
            {
                logo.GetComponent<Image>().sprite = menuSprites["ENGlogo"];
                controls.GetComponent<Image>().sprite = menuSprites["ENGcontrols"];
                startButton.GetComponent<Image>().sprite = menuSprites["ENGstart"];
                optionsButton.GetComponent<Image>().sprite = menuSprites["ENGsettings"];
                quitButton.GetComponent<Image>().sprite = menuSprites["ENGquit"];
            }
            else if (language == "Japanese")
            {
                logo.GetComponent<Image>().sprite = menuSprites["JPNlogo"];
                controls.GetComponent<Image>().sprite = menuSprites["JPNcontrols"];
                startButton.GetComponent<Image>().sprite = menuSprites["JPNstart"];
                optionsButton.GetComponent<Image>().sprite = menuSprites["JPNsettings"];
                quitButton.GetComponent<Image>().sprite = menuSprites["JPNquit"];
            }
            else if (language == "Chinese")
            {
                logo.GetComponent<Image>().sprite = menuSprites["CHNlogo"];
                controls.GetComponent<Image>().sprite = menuSprites["CHNcontrols"];
                startButton.GetComponent<Image>().sprite = menuSprites["CHNstart"];
                optionsButton.GetComponent<Image>().sprite = menuSprites["CHNsettings"];
                quitButton.GetComponent<Image>().sprite = menuSprites["CHNquit"];
            }
        }


        //on 9 key press, settings save delete all data - dev key
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            settingsSave.DeleteData();
        }
    }

    public void StartButton()
    {
        Debug.Log("Start button pressed");
        SceneManager.LoadScene("DevRoom");
    }
    
    public void OptionsButton()
    {
        Debug.Log("Options button pressed");
        SceneManager.LoadScene("OptionMenu");
    }
    
    public void QuitButton()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
    
    public void BackButton()
    {
        Debug.Log("Back button pressed");
        SceneManager.LoadScene("StartMenu");
    }
    
    public void LanguageButton()
    {
        Debug.Log("Language button pressed");
        //move to the next language option in the languages array and change playerpref (loop through)
        
        int currentLanguageIndex = Array.IndexOf(languages, PlayerPrefs.GetString("Language"));
        currentLanguageIndex++;
        currentLanguageIndex %= languages.Length;
        
        settingsSave.language = languages[currentLanguageIndex];
        PlayerPrefs.SetString("Language", settingsSave.language);
        Debug.Log(settingsSave.language);
    }

    public void ContrastButton()
    {
        Debug.Log("Contrast button pressed");
        //switch contrast
        if(PlayerPrefs.GetString("Contrast") == "Normal")
        {
            settingsSave.contrast = "High";
        }
        else
        {
            settingsSave.contrast = "Normal";
        }
    }


    // tien stuff for in game canvas sorry i am making this gross tony
    public void ContinueButton()
    {
        Debug.Log("Continue button pressed");
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
        GameObject.Find("Player").GetComponent<InventoryState>().menuOpen = false;
    }

    public void InGameSettings()
    {
        Debug.Log("hi");
        GameObject.Find("OptionsCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("OptionsCanvas").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("OptionsCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
    }

    public void InGameBack()
    {
        Debug.Log("bye");
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
        GameObject.Find("UICanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("UICanvas").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("UICanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;

    }
}
