using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour, IGenericController
{
    public bool isOn;
    public void Activate() {
        if (!isOn) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            isOn = true;
        } else {
            Reset();
        }
        Debug.Log("Activated");
    }

    public void Reset() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        isOn = false;
    }

    public void Deactivate() {
        
    }
}
