using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    IInteractable objInteract;
    InputAction interactAction;
    // Start is called before the first frame update
    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (objInteract != null && interactAction.triggered)
        {
            objInteract.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        objInteract = collision.transform.GetComponent<IInteractable>();
        if (objInteract != null)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1);
            Debug.Log("Player is in range");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<IInteractable>() != null)
        {
            objInteract = null;
            GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log("Player is out of range");
        }
    }
}
