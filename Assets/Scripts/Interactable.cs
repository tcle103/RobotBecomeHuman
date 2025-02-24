using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public bool inRange;
    public KeyCode interactKey;
    public UnityEvent action;
    private DialogueBox dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange) {
            if (Input.GetKeyDown(interactKey)) {
                if(dialogueBox.IsActive())
                {
                    dialogueBox.Hide();
                } else
                {
                    action.Invoke();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var player = collision.gameObject;
            player.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1);
            inRange = true;
            Debug.Log("Player is in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var player = collision.gameObject;
            player.GetComponent<SpriteRenderer>().color = Color.blue;
            inRange = false;
            Debug.Log("Player is out of range");
        }
    }
}
