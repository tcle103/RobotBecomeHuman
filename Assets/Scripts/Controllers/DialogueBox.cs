using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    
    [SerializeField] Text nameBox;
    [SerializeField] Text dialogueBox;
    public bool textLoaded;
    private string[] dialogue;
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
        if (!textLoaded && nameBox.IsActive())
        {
            //split dialogue box text into the dialogue array based on the seperator $$
            dialogue = dialogueBox.text.Split("$$");
            dialogueBox.text = dialogue[0];
            textLoaded = true;
            
            //print the dialogue array to the console
            // foreach (string line in dialogue)
            // {
            //     Debug.Log(line);
            // }
        }else if (textLoaded && Input.GetKeyDown(interactKey))
        {
            NextDialogue();
        }
    }
    
    private void NextDialogue()
    {
        if (textLoaded && dialogue.Length > 0) {
            for (int i = 0; i < dialogue.Length; i++)
            {
                if (dialogueBox.text == dialogue[i])
                {
                    if (i < dialogue.Length - 1)
                    {
                        dialogueBox.text = dialogue[i + 1];
                    }
                    else
                    {
                        nameBox.gameObject.SetActive(false);
                        dialogueBox.gameObject.SetActive(false);
                        textLoaded = false;
                        dialogue = null;
                    }
                    break;
                }
            }
        }
    }
}