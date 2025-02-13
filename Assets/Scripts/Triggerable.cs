using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
    public UnityEvent action;
    bool isTriggered;
    List<Collider2D> collisions = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collisions.Count == 0) {
            action.Invoke();
            Debug.Log("Player is in range");
        }
        collisions.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        collisions.Remove(collision);
        if (collisions.Count == 0) {
            action.Invoke();
            Debug.Log("Player is out of range");
        }
    }
}
