using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    private string[] scenes;
    private double[] cursorXPos;
    private int cursorPos;
    private RectTransform rt;
    
    // Start is called before the first frame update
    void Start()
    {
        cursorPos = 0;
        if(SceneManager.GetActiveScene().name == "StartMenu")
        {
            scenes = new string[] { "DevRoom", "OptionMenu", "StartMenu" };
        }
        else if (SceneManager.GetActiveScene().name == "OptionMenu")
        {
            scenes = new string[] { "StartMenu", "StartMenu", "StartMenu" };
        }
        else
        {
            scenes = new string[] { "StartMenu", "StartMenu", "StartMenu" };
        }
        cursorXPos = new double[] { -580.7, -574.8, -568.4 };
        //cursorXPos = new double[] { -3, 3.4, 9.9 };
        //change position with rect transform
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2((float)cursorXPos[cursorPos], rt.anchoredPosition.y);
        
        //transform.position = new Vector3((float)cursorXPos[cursorPos], transform.position.y, transform.position.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        //
    }
    
    //press space to go to next scene
    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
        {
            if (cursorPos == 2 && SceneManager.GetActiveScene().name == "StartMenu")
            {
                /*if (Application.isEditor)
                {
                    EditorApplication.isPlaying = false;
                }
                else
                {*/
                    Application.Quit();
                //}
            }
            else
            {
                Application.LoadLevel(scenes[cursorPos]);
            }
        }
        
        //press right or left arrow to move cursor - only work on key press not hold
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D && Input.GetButtonDown("Horizontal"))
        {
            if (cursorPos < 2)
            {
                cursorPos++;
            }
            else
            {
                cursorPos = 0;
            }
            //transform.position = new Vector3((float)cursorXPos[cursorPos], transform.position.y, transform.position.z);
            rt.anchoredPosition = new Vector2((float)cursorXPos[cursorPos], rt.anchoredPosition.y);
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A && Input.GetButtonDown("Horizontal"))
        {
            if (cursorPos > 0)
            {
                cursorPos--;
            }
            else
            {
                cursorPos = 2;
            }
            //transform.position = new Vector3((float)cursorXPos[cursorPos], transform.position.y, transform.position.z);
            rt.anchoredPosition = new Vector2((float)cursorXPos[cursorPos], rt.anchoredPosition.y);
        }
    }
}
