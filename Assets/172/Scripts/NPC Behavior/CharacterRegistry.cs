using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRegistry : MonoBehaviour
{
    public Dictionary<string, bool> characterRegistry = new();

    void Start()
    {

    }

    private string PairToString(int gridX, int gridY)
    {
        return string.Format("{0},{1}", gridX, gridY);
    }

    public bool CheckTile(int gridX, int gridY)
    {
        if (characterRegistry.TryGetValue(PairToString(gridX, gridY), out bool value))
        {
            return value;
        }
        else
        {
            return false;
        }
    }

    public void RegisterTile(int gridX, int gridY)
    {
        characterRegistry.Add(PairToString(gridX, gridY), true);
    }

    public void UnregisterTile(int gridX, int gridY)
    {
        characterRegistry.Remove(PairToString(gridX, gridY));
    }
}
