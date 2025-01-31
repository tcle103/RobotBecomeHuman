using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericController : MonoBehaviour
{
    public bool isOn;
    public void Activate() {
        if (!isOn) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            isOn = true;
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            isOn = false;
        }
        Debug.Log("Activated");
    }
}
