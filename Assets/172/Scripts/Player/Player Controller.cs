using System;
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
    private SaveSystem settingsSave;

    private void Awake(){
        controls = InputSystem.actions.FindAction("Movement");
    }

    void Start()
    {
        settingsSave = FindObjectOfType<SaveSystem>();
        settingsSave.player = transform;
        settingsSave.gameLoad();
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

    private void Update(){
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
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position  + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition)){
            return false;
        }
        else{
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
        StopAllCoroutines();
        transform.position = newPosition;
    }
}
