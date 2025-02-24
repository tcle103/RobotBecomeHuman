using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable enable

public class DialogueBox : MonoBehaviour
{
    
    [SerializeField] Text nameBox;
    [SerializeField] Text dialogueBox;
    
    // Start is called before the first frame update
    void Start()
    {
        nameBox.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }

    public void SetText(string? npcName, string? text)
    {
        nameBox.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(true);
        nameBox.text = npcName;
        dialogueBox.text = text;
    }

    public void Hide()
    {
        nameBox.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return nameBox.gameObject.activeSelf || dialogueBox.gameObject.activeSelf;
    }
}