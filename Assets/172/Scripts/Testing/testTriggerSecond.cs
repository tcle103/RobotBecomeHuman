using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class testTriggerSecond : MonoBehaviour
{
    [SerializeField] private UnityEvent testEvent;
    private bool hasEntered = false;
    [SerializeField] private GameObject[] objects;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasEntered)
        {
            testEvent.Invoke();
            ShowPuzzle();
        }
    }

    public void ShowPuzzle()
    {
        foreach (var obj in objects)
        {
            obj.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            hasEntered = true;
        }
    }
}
