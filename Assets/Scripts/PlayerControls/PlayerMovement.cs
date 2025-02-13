using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;

    InputAction moveAction;
    Rigidbody2D rb;
    DialogueBox canvas;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canvas.textLoaded)
        {
            Vector2 movement = moveAction.ReadValue<Vector2>();
            rb.velocity = movement * speed;
        }
    }
}
