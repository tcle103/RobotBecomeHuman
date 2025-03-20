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
    Animator _playerAnimator;
    SettingsSave settingsSave;
    [SerializeField] private GameObject UIcanvas;
    [SerializeField] private GameObject Optionscanvas;

    // Start is called before the first frame update
    void Start()
    {
        settingsSave = FindObjectOfType<SettingsSave>();
        settingsSave.player = transform;
        settingsSave.gameLoad();
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponent<Animator>();
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
        if ((!dialogueBox.IsActive()) && (inventoryBox == null || !inventoryBox.activeSelf) && !(UIcanvas.GetComponent<CanvasGroup>().interactable) && !(Optionscanvas.GetComponent<CanvasGroup>().interactable))
        {
            Vector2 movement = moveAction.ReadValue<Vector2>();
            if (movement != Vector2.zero && !_audioSource.loop) 
            {
                if (!_audioSource.isPlaying) _audioSource.PlayOneShot(step, 0.1f);
            }
            rb.velocity = movement * speed;
            if (movement == Vector2.up) _playerAnimator.SetTrigger("Up");
            if (movement == Vector2.down) _playerAnimator.SetTrigger("Down");
            if (movement == Vector2.left) _playerAnimator.SetTrigger("Left");
            if (movement == Vector2.right) _playerAnimator.SetTrigger("Right");
        }

    }
}
