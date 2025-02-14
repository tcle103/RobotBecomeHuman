using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour, IGenericController
{
    public bool isOn;
    bool isActive;

    public void Press() {
        if (isActive) {
            if (!isOn) {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                isOn = true;
            } else {
                Reset();
            }
            Debug.Log("Activated");
        }
    }

    public void Activate() {
        isActive = true;
    }
    public void Reset() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        isOn = false;
    }
    public void Deactivate() {
        isActive = false;
    }
}
