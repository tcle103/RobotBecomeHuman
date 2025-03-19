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
    
    // Start is called before the first frame update
    void Start()
    {
        settingsSave = FindObjectOfType<SettingsSave>();
        menuSprites = new Dictionary<String, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Menus");
        foreach (Sprite sprite in sprites)
        {
            menuSprites.Add(sprite.name, sprite);
        }
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
}
