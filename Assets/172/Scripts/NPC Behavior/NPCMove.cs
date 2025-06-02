/* 
 * Last modified by: Ian Stentz
 * Last modified on: 6/2/2025
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
    private Vector3 origPos, targetPos;
    [SerializeField] private float baseSpeed = 0.2f;
    [SerializeField] private List<GameObject> path;
    private int point = 0;
    [SerializeField] private UnityEvent moveCompleteEvent;
    private Rigidbody2D rb;
    //[SerializeField] private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        //playerTransform = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
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
        StartCoroutine(MoveTo(new Vector3(moveX, 0, 0), baseSpeed * Math.Abs(moveX)));
        while (transform.position.x != path[point].transform.position.x)
        {
            yield return null;
        }
        Debug.Log("moving y");
        float moveY = path[point].transform.position.y - transform.position.y;
        StartCoroutine(MoveTo(new Vector3(0, moveY, 0), baseSpeed * Math.Abs(moveY)));
        while (transform.position.y != path[point].transform.position.y)
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

    private IEnumerator MoveTo(Vector3 direction, float speed)
    {
        //float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (Vector3.Dot(targetPos - transform.position, direction) > 0)
        {
            Vector3 nextMove = transform.position + Vector3.Normalize(direction) * speed * Time.deltaTime;
            //switch to rb.MovePosition? or split into individual cells and check if player is in the cell before moving into it?
            rb.MovePosition(nextMove);   
            yield return null;
        }

        // [4/30/25 Tien] just make sure you are precisely
        // at targetPos at the end of Lerp
        transform.position = targetPos;
    }
}
