using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] TextAsset dialogueFile;
    [SerializeField] Text nameBox;
    [SerializeField] Text dialogueBox;
    private int interactionCounter;
    public KeyCode interactKey;
    private bool inRange;

    private DialogNode entryPoint;
    private DialogNode currentNode;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up variables
        interactionCounter = 0;

        if (dialogueFile == null)
        {
            Debug.LogError("Dialogue file not assigned!");
        }
        else if (dialogueBox == null || nameBox == null)
        {
            Debug.LogError("Dialogue box or name box not assigned!");
        }
        else
        {
            entryPoint = DialogParser.Parse(dialogueFile.text);
            currentNode = entryPoint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactKey) && inRange)
        { 
            InteractNPC();
        }
        if(nameBox.IsActive())
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SkipDialog();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChooseOption(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChooseOption(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChooseOption(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChooseOption(3);
            }
        }
    }

    void SkipDialog()
    {
        if (currentNode is DialogChoiceNode choice && choice.options.Count == 0)
        {
            currentNode = currentNode.next;
            UpdateDialog();
        }
    }

    void ChooseOption(int index)
    {
        if (currentNode is DialogChoiceNode choice)
        {
            if (index >= 0 && index < choice.options.Count)
            {
                currentNode = choice.options[index];
                UpdateDialog();
            }
        }
    }

    void UpdateDialog()
    {
        while (true)
        {
            if (currentNode == null)
            {
                // TODO: Make it so the interaction automatically ends if the dialogue ends so they don't have to press E
                break;
            }
            else if (currentNode is DialogChoiceNode choice)
            {
                //Set boxes to text
                dialogueBox.text = choice.text;
                for (int i = 0; i < choice.options.Count; i++)
                {
                    dialogueBox.text += "\n    [" + (i + 1) + "] " + choice.options[i].text;
                }
                break;
            }
            else
            {
                currentNode = currentNode.next;
            }
        }
    }

    public void InteractNPC()
    {
        if (!nameBox.IsActive()) //prevents interaction if dialogue is already active
        {
            //make boxes visible
            nameBox.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);

            currentNode = entryPoint;
            UpdateDialog();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var player = collision.gameObject;
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var player = collision.gameObject;
            inRange = false;
        }
    }
}