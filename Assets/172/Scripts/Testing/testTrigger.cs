using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class testTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent testEvent;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            testEvent.Invoke();
        }
    }
}
