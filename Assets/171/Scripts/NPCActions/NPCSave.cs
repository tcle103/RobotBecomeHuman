using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSave : MonoBehaviour
{
    public static SaveSystem settingsSave;

    // Start is called before the first frame update
    void Start()
    {
        settingsSave = FindObjectOfType<SaveSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void save_point()
    {
        settingsSave.Save();
    }
}
