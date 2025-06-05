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

    private CharacterRegistry characterRegistry;

    private void Awake()
    {
        controls = InputSystem.actions.FindAction("Movement");
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

    private void Start()
    {
        characterRegistry = FindAnyObjectByType<CharacterRegistry>();
        Vector3Int cellCoords = MyWorldToCell(transform.position);
        characterRegistry.RegisterTile(cellCoords.x, cellCoords.y);
    }

    private void Update()
    {
        Vector2 controlValue = controls.ReadValue<Vector2>();

        if (!isMoving && controlValue != Vector2.zero)
        {
            Vector2 dir;
            if (Mathf.Abs(controlValue.x) > Mathf.Abs(controlValue.y)) //future proofing for controller later
            {
                dir = new Vector2(Mathf.Sign(controlValue.x), 0);
            }
            else
            {
                dir = new Vector2(0, Mathf.Sign(controlValue.y));
            }
            if (CanMove(dir))
            {
                StartCoroutine(MovePlayer(dir));
            }
        }
    }

    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = MyWorldToCell(transform.position  + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition)){
            return false;
        }
        else{
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
        StopAllCoroutines();
        transform.position = newPosition;
    }
}
