/*
 * Last modified by: Tien Le
 * Last modified on: 4/23/25
 * 
 * NikoTile.cs contains functionality for a tile that
 * causes a fail state when it is active
 * 
 * Created by: Tien Le
 * Created on: 4/23/25
 * Contributors: Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NikoTile : MonoBehaviour
{
    public bool isActive { get; private set; } = false;
    // [4/23/25 Tien] failEvent should trigger the puzzle manager's
    // fail/reset stuff in response to a fail state
    [SerializeField] private UnityEvent failEvent;
    [SerializeField] private PuzzleZone puzzleZone;

    // [4/23/25 Tien]
    // place a tile to be active
    // can be attached to a unityevent
    // such as being triggered by beatsync
    public void activate()
    {
        isActive = true;
    }

    public void deactivate()
    {
        isActive = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleZone.completed && puzzleZone.puzzleActive)
        {
            if (isActive && other.CompareTag("Player"))
            {
                // [4/23/25 Tien]
                // functionality for player entering an active tile here
                // (fail state)
                failEvent.Invoke();
            }
        }

    }
}
