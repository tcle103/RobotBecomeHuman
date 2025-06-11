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
    public InventoryMenuController inventoryMenuController;

    private InputAction controls;
    // [4/30/25 Tien] this is the "speed" of the player technically
    [SerializeField] private float timeToMove = 0.1f;
    private bool isMoving;
    private Vector3 origPos, targetPos;

    private Animator animator;

    public Vector2 lastMoveDir = Vector2.down; // default facing back (down)
    
    private SaveSystem settingsSave;
    private CharacterRegistry characterRegistry;

    private KeyCode lastKeyPressed;
    private Dictionary<KeyCode, Vector2> keyToDir = new Dictionary<KeyCode, Vector2>
    {
        { KeyCode.W, Vector2.up },
        { KeyCode.S, Vector2.down },
        { KeyCode.A, Vector2.left },
        { KeyCode.D, Vector2.right }
    };

    private void Awake()
    {
        controls = InputSystem.actions.FindAction("Movement");
        animator = GetComponent<Animator>();
    }
    
    void Start() 
    {
        settingsSave = FindObjectOfType<SaveSystem>();
        settingsSave.player = transform;
        settingsSave.gameLoad();
        characterRegistry = GameObject.FindWithTag("TileTracker").GetComponent<CharacterRegistry>();
        Vector3Int cellCoords = MyWorldToCell(transform.position);
        characterRegistry.RegisterTile(cellCoords.x, cellCoords.y);
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
        // Prevent movement when inventory is open
        if (inventoryMenuController != null && inventoryMenuController.isOpen)
        {
            // Set animator to idle with current direction
            animator.SetBool("isMoving", false);
            animator.SetFloat("MoveX", lastMoveDir.x);
            animator.SetFloat("MoveY", lastMoveDir.y);
            return;  // Prevent further movement when inventory is open
        }

        if (Time.timeScale == 0f) return;

        if (!isMoving)
        {
            Vector2 controlValue = controls.ReadValue<Vector2>();
            Vector2 moveDir = Vector2.zero;

            // Prefer last key pressed if it's still being held
            if (Input.GetKey(lastKeyPressed))
            {
                moveDir = keyToDir[lastKeyPressed];
            }
            else
            {
                if (Input.GetKey(KeyCode.W)) moveDir = Vector2.up;
                else if (Input.GetKey(KeyCode.S)) moveDir = Vector2.down;
                else if (Input.GetKey(KeyCode.A)) moveDir = Vector2.left;
                else if (Input.GetKey(KeyCode.D)) moveDir = Vector2.right;
            }

            if (moveDir != Vector2.zero)
            {
                lastMoveDir = moveDir;

                animator.SetBool("isMoving", true);
                animator.SetFloat("MoveX", moveDir.x);
                animator.SetFloat("MoveY", moveDir.y);

                if (CanMove(moveDir))
                    StartCoroutine(MovePlayer(moveDir));
            }
            else
            {
                animator.SetBool("isMoving", false);
                animator.SetFloat("MoveX", lastMoveDir.x);
                animator.SetFloat("MoveY", lastMoveDir.y);
            }
        }
    }
    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = MyWorldToCell(transform.position  + (Vector3)direction);

        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition)){
            return false;
        }
        else
        {
            //also check if the tile is a door
            Vector3 worldCenter = groundTilemap.GetCellCenterWorld(gridPosition);
            if (Physics2D.OverlapPoint(worldCenter, doorLayer))
            {
                return false;
            }
            else
            {
                //[6/2/25 Ian] check if there's an NPC in the way of movement
                // LayerMask npcOnly = LayerMask.GetMask("NPC");
                // bool isNPC = Physics2D.Raycast((Vector2)transform.position - new Vector2(0f, 0.5f), direction, 1.5f, npcOnly);
                // return !isNPC;
                //[6/5/25 Ian] new check
                return !characterRegistry.CheckTile(gridPosition.x, gridPosition.y);
            }
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
        Vector3Int origCell = MyWorldToCell(origPos);
        Vector3Int targetCell = MyWorldToCell(targetPos);
        characterRegistry.RegisterTile(targetCell.x, targetCell.y);

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // [4/30/25 Tien] just make sure you are precisely
        // at targetPos at the end of Lerp
        transform.position = targetPos;
        characterRegistry.UnregisterTile(origCell.x, origCell.y);

        isMoving = false;

    }

    public Vector3Int MyWorldToCell(Vector3 worldPosition)
    {
        return groundTilemap.WorldToCell(worldPosition);
    }

    public void MoveInterrupt(Vector3 newPosition)
    {
        isMoving = false;
        //animator.SetBool("isMoving", false);
        StopAllCoroutines();
        transform.position = newPosition;
    }
}
