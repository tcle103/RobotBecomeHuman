using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    [Serializable]
    public class ExternalAction
    {
        public string name;
        public UnityEvent action;
    }

    [SerializeField] public TextAsset engDialogueFile;
    [SerializeField] public TextAsset jpnDialogueFile;
    [SerializeField] public TextAsset chnDialogueFile;
    public TextAsset dialogueFile;
    public List<TextAsset> dialogs;
    Dictionary<int, TextAsset> hashes = new();
    [SerializeField] List<ExternalAction> externalActions;
    private int interactionCounter;

    private DialogNode entryPoint;
    private DialogNode currentNode;
    private DialogueBox dialogueBox;
    private SaveSystem settingsSave;

    private bool isActive = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        settingsSave = FindObjectOfType<SaveSystem>();
        //settingsSave.npcs.Add(this);
        settingsSave.gameLoad();

        // Fill dictionary with fileID, file pairs
        FillDictionary();
        Debug.Log("Start");
        
        //use settings save to get language
        string language = settingsSave.language;
        Debug.Log("Language: " + language);
        if (language == "English")
        {
            dialogueFile = engDialogueFile;
        }
        else if (language == "Japanese")
        {
            dialogueFile = jpnDialogueFile;
        }
        else if (language == "Chinese")
        {
            dialogueFile = chnDialogueFile;
        }
        else
        {
            dialogueFile = engDialogueFile;
        }
        
        InitDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueBox.IsActive())
        {
            isActive = false;
        }

        if(!isActive)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) //space or A
        {
            SkipDialog();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.JoystickButton4)) //1 or left bumper
        {
            ChooseOption(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.JoystickButton5)) //2 or right bumper
        {
            ChooseOption(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.JoystickButton9)) //3 or left trigger
        {
            ChooseOption(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.JoystickButton10)) //4 or right trigger
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
        if (!isActive)
        {
            isActive = true;
            currentNode = entryPoint;
            UpdateDialog();
        }
    }

    public void InitDialogue() {
        //Set up variables
        dialogueBox = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueBox>();
        interactionCounter = 0;

        if (dialogueFile == null)
        {
            Debug.LogError("Dialogue file not assigned!");
            if (engDialogueFile == null)
            {
                Debug.LogError("Bruh");
            }
        }
        else
        {
            entryPoint = DialogParser.Parse(dialogueFile.text);
            currentNode = entryPoint;
        }
    }

    public TextAsset NPCFileLookup(int fileId)
    {
        if (hashes[fileId])
            return hashes[fileId];
        return null;
    }

    public void FillDictionary()
    {
        for (int i = 0; i < dialogs.Count; i++)
        {
            Debug.Log(dialogs[i].GetInstanceID());
            int fileId = dialogs[i].GetInstanceID();
            hashes[fileId] = dialogs[i];
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}