using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMoveSimple : MonoBehaviour
{
    public float speed = 1;
    InputAction moveAction;
    Rigidbody2D rb;
    public AudioClip step;
    AudioSource _audioSource;
    Animator _playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
