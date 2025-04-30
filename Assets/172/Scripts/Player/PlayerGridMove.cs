/* 
 * Last modified by: Tien Le
 * Last modified on: 4/30/25
 *
 * PlayerGridMove.cs contains functionality for
 * grid-based player movement using the input
 * capturing used in our previous player movement
 * scripts.
 * Grid-based movement informed by this tutorial by Comp-3 Interactive
 * here:
 * https://youtu.be/AiZ4z4qKy44?feature=shared
 *
 * Created by: Tien Le
 * Created on: 4/30/25
 * Contributors: Tien Le, Kellum Inglin, Anthony Garcia (Tony)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerGridMove : MonoBehaviour
{
    InputAction moveAction;
    public AudioClip step;
    AudioSource _audioSource;
    Animator _playerAnimator;

    // [4/30/25 Tien] this is stuff for grid-based movement
    // from the tutorial above
    private bool isMoving;
    // [4/30/25 Tien] this is just for interpolation stuff
    private Vector3 origPos, targetPos;
    // [4/30/25 Tien] this is the "speed" of the player technically
    [SerializeField] private float timeToMove = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // [4/30/25 Tien] this is all stuff for input capturing
        // and anim/sound done by Kellum
        moveAction = InputSystem.actions.FindAction("Move");
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        // [4/30/25 Tien] controller stuff by Tony
        Vector2 controllerMovement;
        if (Gamepad.current == null)
        {
            controllerMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            controllerMovement = Gamepad.current.leftStick.ReadValue();
        }

        // [4/30/25 Tien] just triggers step sounds when walking
        if (movement != Vector2.zero && !_audioSource.loop)
        {
            if (!_audioSource.isPlaying) _audioSource.PlayOneShot(step, 0.1f);
        }
        

        //if ((movement == Vector2.up || (controllerMovement.y > 0.2) && Mathf.Abs(controllerMovement.y) > Mathf.Abs(controllerMovement.x)) && !isMoving)
        //{
        //    Debug.Log("up");
        //    // [4/30/25 Tien] may need to eventually trigger changes
        //    // in the animation when we have actual walking anims
        //    // elsewhere later
        //    _playerAnimator.SetTrigger("Up");
        //    //StartCoroutine(MovePlayer(Vector3.up));
        //}
        //if ((movement == Vector2.down || (controllerMovement.y < -0.2) && Mathf.Abs(controllerMovement.y) > Mathf.Abs(controllerMovement.x)) && !isMoving)
        //{
        //    Debug.Log("down");
        //    _playerAnimator.SetTrigger("Down");
        //    //StartCoroutine(MovePlayer(Vector3.down));
        //}
        //if ((movement == Vector2.left || (controllerMovement.x < 0.2) && Mathf.Abs(controllerMovement.x) > Mathf.Abs(controllerMovement.y)) && !isMoving)
        //{
        //    Debug.Log("left");
        //    _playerAnimator.SetTrigger("Left");
        //    //StartCoroutine(MovePlayer(Vector3.left));
        //}
        //if ((movement == Vector2.right || (controllerMovement.x > -0.2) && Mathf.Abs(controllerMovement.x) > Mathf.Abs(controllerMovement.y)) && !isMoving)
        //{
        //    Debug.Log("right");
        //    _playerAnimator.SetTrigger("Right");
        //    //StartCoroutine(MovePlayer(Vector3.right));
        //}

        if (movement != Vector2.zero && !isMoving)
        {
            StartCoroutine(MovePlayer(new Vector3(movement.x, movement.y, 0)));
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove) 
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // [4/30/25 Tien] just make sure you are precisely
        // at targetPos at the end of Lerp
        transform.position = targetPos;

        isMoving = false;
    }
}
