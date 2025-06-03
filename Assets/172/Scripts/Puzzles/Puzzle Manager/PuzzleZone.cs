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

    private SaveSystem settingsSave;

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
        //find parent of puzzle zone
        PuzzleManager puzzleManager = GetComponentInParent<PuzzleManager>();
        if (puzzleManager != null)
        {
            puzzleManager.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        settingsSave = FindObjectOfType<SaveSystem>();
        if (settingsSave.puzzleZones.Count == 0)
        {
            Debug.Log("No puzzle zones in save file, adding this puzzle zone");
            settingsSave.puzzleZones.Add(this);
        }

        for (int i = 0; i < settingsSave.puzzleZones.Count; i++)
        {
            if (settingsSave.puzzleZones[i] == null)
            {
                Debug.Log("Puzzle " + this.name + " already exists in save file, replacing save");
                settingsSave.puzzleZones[i] = this;
                return;
            }else if (settingsSave.puzzleZones[i].name == this.name)
            {
                return;
            }else if (i == settingsSave.puzzleZones.Count - 1) //last index
            {
                Debug.Log("Puzzlee " + this.name + " does not exist in save file, adding to save");
                settingsSave.puzzleZones.Add(this);
            }
        }
        
        settingsSave.gameLoad();
    }
}
