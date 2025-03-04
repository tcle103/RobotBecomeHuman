using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;

    InputAction moveAction;
    Rigidbody2D rb;
    DialogueBox dialogueBox;
    GameObject inventoryBox;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        dialogueBox = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueBox>();
        inventoryBox = GameObject.FindGameObjectWithTag("Inventory");
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryBox == null)
        {
            inventoryBox = GameObject.FindGameObjectWithTag("Inventory");
        }
        if ((!dialogueBox.IsActive()) && (inventoryBox == null || !inventoryBox.activeSelf))
        {
            Vector2 movement = moveAction.ReadValue<Vector2>();
            rb.velocity = movement * speed;
        }
    }
}
