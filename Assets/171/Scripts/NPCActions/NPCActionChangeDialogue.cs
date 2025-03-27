using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActionChangeScript : MonoBehaviour
{
    [SerializeField] private TextAsset engDialogueFile;
    [SerializeField] private TextAsset jpnDialogueFile;
    [SerializeField] private TextAsset chnDialogueFile;
    [SerializeField] private TextAsset dialogueFile;
    private NPCBehavior npcb;

    public void Start()
    {
        npcb = gameObject.GetComponent<NPCBehavior>();
        //find language in player prefs
        string language = PlayerPrefs.GetString("Language");
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
    }
    
    public void ChangeDialogue()
    {
        npcb.dialogueFile = this.dialogueFile;
        npcb.InitDialogue();
        Debug.Log("BEEP");
    }
}
