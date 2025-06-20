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
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleZone puzzleZone;
    [SerializeField] private bool anyTileSolution;
    [SerializeField] private List<DaleTile> solution;
    [SerializeField] private GameObject resetPoint;
    [SerializeField] private UnityEvent successEvent;
    private int failCount = 0;
    private GameObject player;
    
    private float timeStart = 0;
    private float timeEnd = 0;
    private float frameTimeStart = 0;
    private float frameTimeEnd = 0;

    private PuzzleStats puzzleStats;
    
    private SaveSystem settingsSave;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        puzzleStats = GameObject.Find("Analytics").GetComponent<PuzzleStats>();
        settingsSave = FindObjectOfType<SaveSystem>();
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
                frameTimeEnd = Time.deltaTime;
            }
            if (anyTileSolution)
            {
                if (solution.Count > 0)
                {
                    foreach (DaleTile tile in solution)
                    {
                        if (tile.isActive)
                        {
                            puzzleSuccess();
                        }

                    }
                }
            }
            else
            {
                if (solution.Count > 0)
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
        frameTimeEnd = Time.deltaTime;
        float timeTaken = timeEnd - timeStart;
        float frameTimeTaken = 1.0f / (frameTimeEnd - frameTimeStart);

        settingsSave.analyticsManager.SendPuzzleSolvedEvent(timeTaken, failCount);
        puzzleStats.PuzzleUpdate(timeTaken, failCount);
        puzzleStats.FrameRateUpdate(frameTimeTaken);
        Debug.Log("Puzzle solved in " + timeTaken + " seconds with " + failCount + " fails.");
        // Reset
        timeStart = 0;
        timeEnd = 0;
        failCount = 0;
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
        failCount++;
    }

}
