using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class testTriggerDisable : MonoBehaviour
{
    [SerializeField] private UnityEvent testEvent;
    private bool hasEntered = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            testEvent.Invoke();
            hasEntered = true;
        }
    }
}
