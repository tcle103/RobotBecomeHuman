/*
 * Last modified by: Tien Le
 * Last modified on: 4/23/25
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

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleZone puzzleZone;
    [SerializeField] private List<DaleTile> solution;
    [SerializeField] private GameObject resetPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // [4/23/25 Tien] puzzle validation
        if (puzzleZone.puzzleActive && !puzzleZone.completed)
        {
            foreach (DaleTile tile in solution) 
            {
                if (!tile.isActive)
                {
                    return;
                }
            }
            
        }
    }

    // [4/23/25 Tien] 
    // things that happen when puzzle is successfully solved
    public void puzzleSuccess()
    {
        puzzleZone.setComplete();
    }

    // [4/23/25 Tien]
    // things that happen when a fail state is detected
    // ex. resetting the player, etc.
    public void puzzleFail()
    {

    }

}
