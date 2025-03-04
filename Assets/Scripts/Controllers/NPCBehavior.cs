using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    [Serializable]
    public class ExternalAction
    {
        public string name;
        public UnityEvent action;
    }


    [SerializeField] TextAsset dialogueFile;
    [SerializeField] List<ExternalAction> externalActions;
    private int interactionCounter;

    private DialogNode entryPoint;
    private DialogNode currentNode;
    private DialogueBox dialogueBox;

    private bool isActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up variables
        dialogueBox = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueBox>();
        interactionCounter = 0;

        if (dialogueFile == null)
        {
            Debug.LogError("Dialogue file not assigned!");
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
        if(!dialogueBox.IsActive())
        {
            isActive = false;
        }

        if(!isActive)
        {
            return;
        }

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
                dialogueBox.Hide();
                isActive = false;
                break;
            }
            else if (currentNode is DialogChoiceNode choice)
            {
                //Set boxes to text
                string text = choice.text ?? "";
                for (int i = 0; i < choice.options.Count; i++)
                {
                    text += "\n    [" + (i + 1) + "] " + choice.options[i].text;
                }
                dialogueBox.SetText(choice.name, text);
                break;
            }
            else if (currentNode is DialogGoToNode goToNode)
            {
                dialogueBox.Hide();
                isActive = false;
                foreach (ExternalAction action in externalActions)
                {
                    if (action.name == goToNode.name)
                    {
                        action.action.Invoke();
                    }
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
        isActive = true;
        currentNode = entryPoint;
        UpdateDialog();
    }
}