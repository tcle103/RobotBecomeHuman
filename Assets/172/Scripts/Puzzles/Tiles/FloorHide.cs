using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.Tilemaps;

public class FloorHide : MonoBehaviour
{
    //private bool hide = true;
    private TilemapRenderer floorTileData;

    // Start is called before the first frame update
    void Start()
    {
        floorTileData = GetComponent<TilemapRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGrid()
    {
        //hide = false;
        if (floorTileData)
        {
            Debug.Log("hi");
            floorTileData.rendererPriority = 0;
        }
    }
}
