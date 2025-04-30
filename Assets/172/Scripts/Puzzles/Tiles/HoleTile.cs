/*
 * Last modified by: Tien Le
 * Last modified on: 4/24/25
 * 
 * HoleTile.cs contains functionality for a tile that
 * will just kill you when you touch it
 * 
 * Created by: Tien Le
 * Created on: 4/24/25
 * Contributors: Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoleTile : MonoBehaviour
{
    [SerializeField] private UnityEvent failEvent;
    [SerializeField] private PuzzleZone puzzleZone;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!puzzleZone.completed && puzzleZone.puzzleActive)
        {
            if (other.CompareTag("Player"))
            {
                failEvent.Invoke();
            }
        }

    }
}
