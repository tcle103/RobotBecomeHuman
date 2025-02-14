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
            Debug.Log("Run");
            //make boxes visible
            nameBox.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);

            if (currentNode == null)
            {
                currentNode = entryPoint;
            }

            while (true)
            {
                if(currentNode == null)
                {
                    break;
                }
                else if (currentNode is DialogChoiceNode choice)
                {
                    //Set boxes to text
                    //nameBox.text = npcName;
                    dialogueBox.text = choice.text;
                    break;
                }
                else
                {
                    currentNode = currentNode.next;
                }
            }
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