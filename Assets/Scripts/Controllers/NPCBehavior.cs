using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] TextAsset dialogueFile;
    [SerializeField] Text nameBox;
    [SerializeField] Text dialogueBox;
    private string npcName;
    private string[] dialogue;
    private int interactionCounter;
    public KeyCode interactKey;
    private bool inRange;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up variables
        npcName = "";
        dialogue = Array.Empty<String>();
        interactionCounter = 0;
        
        if(dialogueFile == null || dialogueFile.text == "")
        {
            Debug.LogError("Dialogue file not assigned or empty!");
        }else if(dialogueFile.text.Split('\n').Length < 2)
        {
            Debug.LogError("Dialogue file must contain at least a name and one line of dialogue!");
        }else if(dialogueBox == null || nameBox == null)
        {
            Debug.LogError("Dialogue box or name box not assigned!");
        }else
        {
            //Set name to the first line of the text file
            string[] lines = dialogueFile.text.Split('\n');
            npcName = lines[0];
            
            //Set dialogue to the rest of the text file
            dialogue = lines[1..];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactKey) && inRange)
        { 
            InteractNPC();
        }    
    }
    
    public void InteractNPC()
    {
        if (!nameBox.IsActive()) //prevents interaction if dialogue is already active
        {
            //make boxes visible
            nameBox.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);

            //Set boxes to text
            nameBox.text = npcName;
            dialogueBox.text = dialogue[interactionCounter]; //dialogue will be handled by dialogue script

            //Incrementing dialogue counter
            interactionCounter++;
            if (interactionCounter >= dialogue.Length)
            {
                interactionCounter = 0;
            }
        }
    }
    
    public string GetName()
    {
        return npcName;
    }
    
    public string[] GetDialogue()
    {
        return dialogue;
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