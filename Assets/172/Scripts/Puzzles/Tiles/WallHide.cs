using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class WallHide : MonoBehaviour
{
    private bool hide = true;
    private TilemapRenderer floorTileData;

    // Start is called before the first frame update
    void Start()
    {
        floorTileData = GameObject.Find("wall (hidden)").GetComponent<TilemapRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hide)
        {
            floorTileData.sortingOrder = -1;
        }
        if (hide)
        {
            floorTileData.sortingOrder = -2;
        }
    }

    public void ShowGrid()
    {
        Debug.Log("hi");
        hide = false;
    }
}
