using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;
    InputAction moveAction;
    Rigidbody2D rb;
    DialogueBox dialogueBox;
    GameObject inventoryBox;
    public AudioClip step;
    AudioSource _audioSource;
    Animator _playerAnimator;
    SaveSystem settingsSave;
    [SerializeField] private GameObject UIcanvas;
    [SerializeField] private GameObject Optionscanvas;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject credits;
    private Dictionary<String, Sprite> creditsSprites;

    // Start is called before the first frame update
    void Start()
    {
        settingsSave = FindObjectOfType<SaveSystem>();
        settingsSave.player = transform;
        settingsSave.gameLoad();
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponent<Animator>();
        dialogueBox = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DialogueBox>();
        inventoryBox = GameObject.FindGameObjectWithTag("Inventory");

        creditsSprites = new Dictionary<String, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Credits");
        foreach (Sprite sprite in sprites)
        {
            creditsSprites.Add(sprite.name, sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryBox == null)
        {
            inventoryBox = GameObject.FindGameObjectWithTag("Inventory");
        }
        if ((!dialogueBox.IsActive()) && (inventoryBox == null || !inventoryBox.activeSelf) && !(UIcanvas.GetComponent<CanvasGroup>().interactable) && !(Optionscanvas.GetComponent<CanvasGroup>().interactable))
        {
            Vector2 movement = moveAction.ReadValue<Vector2>();
            Vector2 controllerMovement;
            if (Gamepad.current == null)
            {
                controllerMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            else
            {
                controllerMovement = Gamepad.current.leftStick.ReadValue();
            }
            if (movement != Vector2.zero && !_audioSource.loop) 
            {
                if (!_audioSource.isPlaying) _audioSource.PlayOneShot(step, 0.1f);
            }
            rb.velocity = movement * speed;
            if (movement == Vector2.up || (controllerMovement.y > 0.2) && Mathf.Abs(controllerMovement.y) > Mathf.Abs(controllerMovement.x)) _playerAnimator.SetTrigger("Up");
            if (movement == Vector2.down || (controllerMovement.y < -0.2) && Mathf.Abs(controllerMovement.y) > Mathf.Abs(controllerMovement.x)) _playerAnimator.SetTrigger("Down");
            if (movement == Vector2.left || (controllerMovement.x < 0.2) && Mathf.Abs(controllerMovement.x) > Mathf.Abs(controllerMovement.y)) _playerAnimator.SetTrigger("Left");
            if (movement == Vector2.right || (controllerMovement.x > -0.2) && Mathf.Abs(controllerMovement.x) > Mathf.Abs(controllerMovement.y)) _playerAnimator.SetTrigger("Right");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            string language = settingsSave.language;
            if (language == "English")
            {
                credits.GetComponent<SpriteRenderer>().sprite = creditsSprites["en_credits"];
            }
            else if (language == "Japanese")
            {
                credits.GetComponent<SpriteRenderer>().sprite = creditsSprites["jp_credits"];
            }
            else if (language == "Chinese")
            {
                credits.GetComponent<SpriteRenderer>().sprite = creditsSprites["cn_credits"];
            }
            Debug.Log("Transition!");
            overlay.GetComponent<FadeIn>().Activate();
            credits.GetComponent<FadeIn>().Activate();
        }
    }
}
