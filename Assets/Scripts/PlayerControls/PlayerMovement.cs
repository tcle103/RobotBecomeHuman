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
    public AudioClip step;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
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
            if (movement != Vector2.zero && !_audioSource.loop) 
            {
                if (!_audioSource.isPlaying) _audioSource.PlayOneShot(step, 0.1f);
            }
            rb.velocity = movement * speed;
        }
    }
}
