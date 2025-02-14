using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    
    [SerializeField] Text nameBox;
    [SerializeField] Text dialogueBox;
    public bool textLoaded;
    public KeyCode interactKey;
    
    // Start is called before the first frame update
    void Start()
    {
        textLoaded = false;
        nameBox.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            textLoaded = !textLoaded;
            nameBox.gameObject.SetActive(textLoaded);
            dialogueBox.gameObject.SetActive(textLoaded);
        }
    }
}