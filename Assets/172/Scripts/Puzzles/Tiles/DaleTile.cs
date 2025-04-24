/*
 * Last modified by: Tony Garcia
 * Last modified on: 4/24/25
 * 
 * DaleTile.cs contains tile functionality that activates (and stays active)
 * on entry
 * Reentry  is considered a fail state
 * 
 * Created by: Tien Le
 * Created on: 4/23/25
 * Contributors: Tien Le
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DaleTile : MonoBehaviour
{
    public bool isActive { get; private set; } = false;
    // [4/23/25 Tien] failEvent should trigger the puzzle manager's
    // fail/reset stuff in response to a fail state
    [SerializeField] private UnityEvent failEvent;
    [SerializeField] private PuzzleZone puzzleZone;
    private SpriteRenderer spriteRenderer;
    
    // [4/24/25 Tony] adding in neighbor functionality from 171->Scripts->Tile.cs
    Collider2D _colliderSelf;
    Collider2D _colliderOther;
    [SerializeField] List<GameObject> _neighbors;
    public bool isRootTile;
    public bool onTile;
    private float requiredOverlap = 0.25f; // percentage of overlap required to be considered on the tile

    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        
        //[4/24/25 Tony] adding in neighbor functionality from 171->Scripts->Tile.cs
        _colliderSelf = GetComponent<Collider2D>();
        FindNeighbors();
    }

    void Update()
    {
        // [4/24/25 Tony] ensures that player can't be on two tiles at once
        if (_colliderOther && _colliderOther.CompareTag("Player"))
        {
            // [4/24/25 Tony] checks if the player is on the tile
            //  either by having the center of the player bounds(fast detection) on the tile bounds
            //  or by checking the percentage of overlap between the two colliders 
            if ((_colliderSelf.bounds.Contains(_colliderOther.bounds.center) || (CalculateOverlapPercentage(_colliderSelf, _colliderOther) >= requiredOverlap)))
            {
                if (!isRootTile)
                {
                    isRootTile = true;
                }
                if (!onTile) activate();
                onTile = true;
                return;
            }
            isRootTile = false;
        }
        
        if (onTile && !isRootTile)
        {
            onTile = false;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_colliderOther) _colliderOther = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_colliderOther == other) _colliderOther = null;
    }

    public void activate()
    {
        if (!puzzleZone.completed && puzzleZone.puzzleActive)
        {
            if (!isActive)
            {
                // [4/23/25 Tien]
                // functionality for activating tile here
                // (ex. changing the sprite to reflect active state)
                spriteRenderer.color = Color.green;
                isActive = true;
            }
            else
            {
                // [4/23/25 Tien]
                // functionality for reentry here

                failEvent.Invoke();
            }
        }
    }

    public void deactivate()
    {
        isActive = false;
        isRootTile = false;
        spriteRenderer.color = Color.red;
    }
    
    //[4/24/25 Tony] adding in neighbor functionality from 171->Scripts->Tile.cs
    private void FindNeighbors()
    {
        _neighbors = new();

        Vector2[] directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(_colliderSelf.bounds.center, dir);
            if (hit && hit.collider.gameObject.GetComponent<DaleTile>())
            {
                //Debug.Log("Hit " + hit.collider.gameObject);
                _neighbors.Add(hit.collider.gameObject);
            }
        }
    }
    
    // [4/24/25 Tony] Calculates the percentage of overlap between two colliders
    // the purpose of this is to make it so that being on the tile is more forgiving than just
    //  needing the center of the player collider to be on the tile
    private float CalculateOverlapPercentage(Collider2D colliderA, Collider2D colliderB)
    {
        //Gets bounds of player and tile colliders
        Bounds boundsA = colliderA.bounds;
        Bounds boundsB = colliderB.bounds;

        // Calculate intersection extents
        float overlapX = Mathf.Max(0, Mathf.Min(boundsA.max.x, boundsB.max.x) - Mathf.Max(boundsA.min.x, boundsB.min.x));
        float overlapY = Mathf.Max(0, Mathf.Min(boundsA.max.y, boundsB.max.y) - Mathf.Max(boundsA.min.y, boundsB.min.y));

        //Gets area of intersection and area of tile
        float areaIntersection = overlapX * overlapY;
        float areaB = boundsB.size.x * boundsB.size.y;
        
        //Returns the percentage of overlap - if over threshold it will pass
        return areaIntersection / areaB;
    }
}
