using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActionChangeScript : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueFile;
    private NPCBehavior npcb;

    public void Start()
    {
        npcb = gameObject.GetComponent<NPCBehavior>();
    }
    public void ChangeDialogue()
    {
        npcb.dialogueFile = this.dialogueFile;
        npcb.Start();
        Debug.Log("BEEP");
    }
}
