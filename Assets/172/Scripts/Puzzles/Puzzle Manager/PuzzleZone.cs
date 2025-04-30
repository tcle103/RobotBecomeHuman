/*
 * Last modified by: Tien Le
 * Last modified on: 4/23/25
 * 
 * PuzzleZone.cs contains functionality for detecting when the
 * player is in a given puzzle zone and reporting that to the
 * puzzle manager
 * 
 * Created by: Tien Le
 * Created on: 4/23/25
 * Contributors: Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleZone : MonoBehaviour
{
    public bool puzzleActive { get; private set; } = false;
    public bool completed { get; private set; } = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !completed)
        {
            Debug.Log("puzzle active");
            puzzleActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !completed)
        {
            Debug.Log("puzzle inactive");
            puzzleActive = false;
        }
    }

    public void setComplete()
    {
        completed = true;
    }
}
