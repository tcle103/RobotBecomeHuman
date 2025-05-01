using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    private PlayerMovement controls;
    // [4/30/25 Tien] this is the "speed" of the player technically
    [SerializeField] private float timeToMove = 0.1f;
    private bool isMoving;
    private Vector3 origPos, targetPos;

    private void Awake(){
        controls = new PlayerMovement();        
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
       controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction){
        if (CanMove(direction) && !isMoving){
            //transform.position += (Vector3)direction;
            StartCoroutine(MovePlayer((Vector3)direction));
        }
    }

    private bool CanMove(Vector2 direction) {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position  + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition)){
            return false;
        }
        else{
            return true;
        }
    }

    // [4/30/25 Tien] this is basically equivalent
    // to a "transform.position += direction"
    // direction being a Vector3, usually like. 
    // Vector3.up, etc.
    // it just does it over a small period of time
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

    public void MoveInterrupt()
    {
        isMoving = false;
        StopAllCoroutines();
    }
}
