using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    List<Tile> _tiles;
    readonly float tileMargin = 2.25f;
    bool isComplete;
    public UnityEvent action;

    // Start is called before the first frame update
    void Start()
    {
        _tiles = new();
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isComplete)
        {
            foreach (var tile in _tiles)
            {
                if (!tile.isActivated)
                    return;
            }
            action.Invoke();
            isComplete = true;
            Debug.Log("Complete");
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var tilePosX = transform.position.x + (i * tileMargin);
                var tilePosY = transform.position.y + (j * tileMargin);
                var tilePos = new Vector3(tilePosX, tilePosY);

                var newTile = Instantiate(_tilePrefab, tilePos, Quaternion.identity);
                newTile.name = $"Tile {i} {j}";

                _tiles.Add(newTile);
            }
        }
    }

    public void Reset() {
        foreach (var tile in _tiles)
            tile.Deactivate();
    }

    Tile GetTileAtPos(Vector3 pos)
    {
        return null;
    }

    Tile[] GetNeighboringTiles(Tile tile)
    {
        return null;
    }
}