/* 
 * Last modified by: Tien Le
 * Last modified on: 4/30/25
 *
 * NPCMove.cs contains functionality for
 * grid-based NPC movement.
 * Grid-based movement based on this tutorial by Comp-3 Interactive
 * here:
 * https://youtu.be/AiZ4z4qKy44?feature=shared
 *
 * Created by: Tien Le
 * Created on: 4/30/25
 * Contributors: Tien Le, Kellum Inglin, Anthony Garcia (Tony)
 */

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
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
        StartCoroutine(MoveLerp(new Vector3(moveX, 0, 0), baseSpeed * Math.Abs(moveX)));
        while (transform.position.x != path[point].transform.position.x)
        {
            yield return null;
        }
        Debug.Log("moving y");
        float moveY = path[point].transform.position.y - transform.position.y;
        StartCoroutine(MoveLerp(new Vector3(0, moveY, 0), baseSpeed * Math.Abs(moveY)));
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

    }

    private IEnumerator MoveLerp(Vector3 direction, float timeToMove)
    {
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
    }
}
