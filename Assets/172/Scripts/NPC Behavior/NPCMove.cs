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
    private bool isMoving;
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
                StartCoroutine(MoveTo(subDirection, speed));
                sumDist += tileSize;
                //Debug.Log(sumDist);
                //Debug.Log(subDirection);
            }
            yield return null;
        }
    }

    private IEnumerator MoveTo(Vector3 direction, float speed)
    {
        //float elapsedTime = 0;

        tileMove = true;

        origPos = transform.position;
        targetPos = origPos + direction;
        Vector3Int origCell = player.MyWorldToCell(origPos);
        Vector3Int targetCell = player.MyWorldToCell(targetPos);
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
}
