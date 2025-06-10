/* 
 * Last modified by: Ian Stentz
 * Last modified on: 6/5/2025
 *
 * NPCMove.cs contains functionality for
 * grid-based NPC movement.
 * Grid-based movement based on this tutorial by Comp-3 Interactive
 * here:
 * https://youtu.be/AiZ4z4qKy44?feature=shared
 *
 * Created by: Tien Le
 * Created on: 5/14/25
 * Contributors: Tien Le, Kellum Inglin, Anthony Garcia (Tony), Ian Stentz
 */

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCMove : MonoBehaviour
{
    public bool activeMove;
    public bool loop;
    private bool pathComplete;
    [SerializeField] private bool isMoving;
    private bool tileMove = false;
    private Vector3 origPos, targetPos;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private List<GameObject> path;
    private int point = 0;
    //[Ian 6/5/25] Wish there was a better way to do this...
    private float tileSize = 1f;
    [SerializeField] private UnityEvent moveCompleteEvent;
    private PlayerController player;
    private CharacterRegistry characterRegistry;
    //[SerializeField] private float speed = 5f;
    public bool stalled = false;
    private Vector3Int origCell, targetCell;
    private Vector3 currdir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        characterRegistry = GameObject.FindWithTag("TileTracker").GetComponent<CharacterRegistry>();
        Vector3Int cellCoords = player.MyWorldToCell(transform.position);
        characterRegistry.RegisterTile(cellCoords.x, cellCoords.y);
        Debug.Log("Registered!");
    }

    // Update is called once per frame
    void Update()
    {
        if (activeMove && !isMoving)
        {
            isMoving = true;
            StartCoroutine(Move(path));
        }
        if (!activeMove)
        {
            isMoving = false;
        }
    }

    private IEnumerator Move(List<GameObject> path)
    {
        Debug.Log("moving x");
        Debug.Log(path[point].transform.position.x);
        float moveX = path[point].transform.position.x - transform.position.x;
        StartCoroutine(SplitTiles(new Vector3(moveX, 0, 0), baseSpeed));
        while (Math.Abs(transform.position.x - path[point].transform.position.x) > 0.00001)
        {
            yield return null;
        }
        Debug.Log("moving y");
        float moveY = path[point].transform.position.y - transform.position.y;
        StartCoroutine(SplitTiles(new Vector3(0, moveY, 0), baseSpeed));
        while (Math.Abs(transform.position.y - path[point].transform.position.y) > 0.00001)
        {
            yield return null;
        }
        Debug.Log("here");
        if (point < path.Count - 1)
        {
            ++point;
            StartCoroutine(Move(path));
        }
        else if (loop)
        {
            path.Reverse();
            point = 0;
            StartCoroutine(Move(path));
        }
        else
        {
            moveCompleteEvent.Invoke();
        }

    }

    private IEnumerator SplitTiles(Vector3 direction, float speed)
    {
        float sumDist = 0f;
        Vector3 subDirection;
        Vector3Int nextTile;
        Vector3 targetPos = transform.position + direction;
        while (sumDist < direction.magnitude)
        {
            if (direction.magnitude - sumDist >= tileSize)
            {
                subDirection = direction.normalized * tileSize;
            }
            else
            {
                subDirection = direction.normalized * (direction.magnitude - sumDist);
            }
            nextTile = player.MyWorldToCell(transform.position + subDirection);
            //could also do recursively?
            if (!tileMove && !characterRegistry.CheckTile(nextTile.x, nextTile.y))
            {
                stalled = false;
                StartCoroutine(MoveTo(subDirection, speed));
                sumDist += tileSize;
                //Debug.Log(sumDist);
                //Debug.Log(subDirection);
            }
            else if (!tileMove && characterRegistry.CheckTile(nextTile.x, nextTile.y)) {
                stalled = true;
            }
            yield return null;
        }
    }

    private IEnumerator MoveTo(Vector3 direction, float speed)
    {
        //float elapsedTime = 0;
        currdir = direction;
        tileMove = true;

        origPos = transform.position;
        targetPos = origPos + direction;
        origCell = player.MyWorldToCell(origPos);
        targetCell = player.MyWorldToCell(targetPos);
        characterRegistry.RegisterTile(targetCell.x, targetCell.y);

        while (Vector3.Dot(targetPos - transform.position, direction) > 0)
        {
            Vector3 nextMove = transform.position + speed * Time.deltaTime * Vector3.Normalize(direction);
            //switch to rb.MovePosition? or split into individual cells and check if player is in the cell before moving into it?
            transform.position = nextMove;
            yield return null;
        }

        // [4/30/25 Tien] just make sure you are precisely
        // at targetPos at the end of Lerp
        transform.position = targetPos;
        characterRegistry.UnregisterTile(origCell.x, origCell.y);
        tileMove = false;
    }

    public void MoveToInterrupt() 
    {
        characterRegistry.UnregisterTile(origCell.x, origCell.y);
        characterRegistry.UnregisterTile(targetCell.x, targetCell.y);
        if (Vector3.Dot(targetPos - transform.position, currdir) < Vector3.Dot(origPos - transform.position, -currdir))
        {
            StartCoroutine(simpleLerpMove(transform.position, targetPos));
        }
        else
        {
            StartCoroutine(simpleLerpMove(transform.position, origPos));
        }
        
        tileMove = false;
    }

    private IEnumerator simpleLerpMove(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0;
        float timeToMove = 0.2f;
        Vector3Int startcell = player.MyWorldToCell(start);
        Vector3Int endcell = player.MyWorldToCell(end);
        if (!characterRegistry.CheckTile(endcell.x, endcell.y))
        {
            characterRegistry.RegisterTile(endcell.x, endcell.y);
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        characterRegistry.UnregisterTile(startcell.x, startcell.y);

    }
}
