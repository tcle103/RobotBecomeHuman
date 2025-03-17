using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSave : MonoBehaviour
{
    private static GameObject instance;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        language = "English";
        contrast = "Normal";
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.SetString("Contrast", contrast);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.SetString("Contrast", contrast);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    
    public string language { get; set; } = "English";
    public string contrast { get; set; } = "Normal";
}
