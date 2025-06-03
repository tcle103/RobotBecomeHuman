using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] LayerMask doorLayer;

    private InputAction controls;
    // [4/30/25 Tien] this is the "speed" of the player technically
    [SerializeField] private float timeToMove = 0.1f;
    private bool isMoving;
    private Vector3 origPos, targetPos;

    private Animator animator;
    private Vector2 lastMoveDir = Vector2.down; // default facing back (down)



    private void Awake()
    {
        controls = InputSystem.actions.FindAction("Movement");
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Vector2 controlValue = controls.ReadValue<Vector2>();

        if (!isMoving && controlValue != Vector2.zero)
        {
            Vector2 dir;
            if (Mathf.Abs(controlValue.x) > Mathf.Abs(controlValue.y))
                dir = new Vector2(Mathf.Sign(controlValue.x), 0);
            else
                dir = new Vector2(0, Mathf.Sign(controlValue.y));

            lastMoveDir = dir;

            animator.SetBool("isMoving", true);
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);

            if (CanMove(dir))
                StartCoroutine(MovePlayer(dir));
        }
        else if (!isMoving && controlValue == Vector2.zero)
        {
            animator.SetBool("isMoving", false);
            // Keep player facing same direction when idle
            animator.SetFloat("MoveX", lastMoveDir.x);
            animator.SetFloat("MoveY", lastMoveDir.y);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        {
            return false;
        }
        else
        {
            //also check if the tile is a door
            Vector3 worldCenter = groundTilemap.GetCellCenterWorld(gridPosition);
            return !Physics2D.OverlapPoint(worldCenter, doorLayer);
        }
    }

    // [4/30/25 Tien] this is basically equivalent
    // to a "transform.position += direction"
    // direction being a Vector3, usually like. 
    // Vector3.up, etc.
    // it just does it over a small period of time
    private IEnumerator MovePlayer(Vector2 direction)
    {
        isMoving = true;

        // Set animation parameters *here*, only if we're actually going to move
        animator.SetBool("isMoving", true);
        animator.SetFloat("MoveX", (int)direction.x);
        animator.SetFloat("MoveY", (int)direction.y);

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + (Vector3)direction;

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



    public void MoveInterrupt(Vector3 newPosition)
    {
        isMoving = false;
        //animator.SetBool("isMoving", false);
        StopAllCoroutines();
        transform.position = newPosition;
    }
}
