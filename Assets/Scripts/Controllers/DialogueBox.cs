using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable enable

public class DialogueBox : MonoBehaviour
{
    
    [SerializeField] Text nameBox;
    [SerializeField] Text dialogueBox;
    [SerializeField] GameObject dialoguePanel;
    
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void SetText(string? npcName, string? text)
    {
        dialoguePanel.SetActive(true);
        nameBox.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(true);
        nameBox.text = npcName;
        dialogueBox.text = text;
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
        nameBox.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return nameBox.gameObject.activeSelf || dialogueBox.gameObject.activeSelf;
    }
}