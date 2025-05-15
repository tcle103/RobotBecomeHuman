/*
 * Last modified by: Tien Le
 * Last modified on: 4/24/25
 * 
 * PuzzleManager.cs contains functionality for fail state (reset)
 * and validation of tiles
 * 
 * Created by: Tien Le
 * Created on: 4/23/25
 * Contributors: Tien Le
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleZone puzzleZone;
    [SerializeField] private bool anyTileSolution;
    [SerializeField] private List<DaleTile> solution;
    [SerializeField] private GameObject resetPoint;
    [SerializeField] private UnityEvent successEvent;
    private int FailCount = 0;
    private GameObject player;
    
    private float timeStart = 0;
    private float timeEnd = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        // [4/23/25 Tien] puzzle validation
        if (puzzleZone.puzzleActive && !puzzleZone.completed)
        {
            //start timer if not started already
            if (timeStart == 0)
            {
                timeStart = Time.time;
            }
            if (anyTileSolution)
            {
                foreach (DaleTile tile in solution)
                {
                    if (tile.isActive)
                    {
                        puzzleSuccess();
                    }

                }
            }
            else
            {
                foreach (DaleTile tile in solution)
                {
                    if (!tile.isActive)
                    {
                        return;
                    }

                }
                puzzleSuccess();
            }
        }
    }

    // [4/23/25 Tien] 
    // things that happen when puzzle is successfully solved
    public void puzzleSuccess()
    {
        puzzleZone.setComplete();
        successEvent.Invoke();
        Component[] nikoTiles = GetComponentsInChildren<NikoTile>();
        foreach (NikoTile tile in nikoTiles)
        {
            tile.deactivate();
        }
        timeEnd = Time.time;
        float timeTaken = timeEnd - timeStart;
        
        // Write time taken and fail count to a file
        string filePath = Application.persistentDataPath + "/puzzle_data.txt";
        System.IO.File.AppendAllText(filePath, "Puzzle solved in " + timeTaken + " seconds with " + FailCount + " fails.\n");
        Debug.Log("Puzzle solved in " + timeTaken + " seconds with " + FailCount + " fails.");
        // Reset
        timeStart = 0;
        timeEnd = 0;
        FailCount = 0;
    }

    // [4/23/25 Tien]
    // things that happen when a fail state is detected
    // ex. resetting the player, etc.
    public void puzzleFail()
    {
        player.transform.position = resetPoint.transform.position;
        // stop player movement coroutine in the player controller class
        player.GetComponent<PlayerController>().MoveInterrupt(resetPoint.transform.position);
        Component[] daleTiles = GetComponentsInChildren<DaleTile>();
        foreach (DaleTile tile in daleTiles)
        {
            tile.deactivate();
        }
        FailCount++;
    }

}
