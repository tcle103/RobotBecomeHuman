/*
 * Last modified by: Tien Le
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

    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleZone.completed && puzzleZone.puzzleActive)
        {
            if (!isActive && other.CompareTag("Player"))
            {
                // [4/23/25 Tien]
                // functionality for activating tile here
                // (ex. changing the sprite to reflect active state)
                spriteRenderer.color = Color.green;
                isActive = true;
            }
            else if (isActive && other.CompareTag("Player"))
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
        spriteRenderer.color = Color.red;
    }
}
