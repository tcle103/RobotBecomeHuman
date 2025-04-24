using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEventScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printTest()
    {
        Debug.Log("test event trigger!!!!");
    }

    public void printFail()
    {
        Debug.Log("puzzle fail state detected, replace me with the puzzle manager fail function later!");
    }

    public void printSuccess() 
    {
        Debug.Log("puzzle successs state detected, replace me with the puzzle manager success function later!");
    }
}
