using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Collider2D doorCollider;
    bool isOpen;
    // Start is called before the first frame update
    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Operate() {
        var mat = GetComponent<Renderer>().material;
        Color newColor;
        if (!isOpen) {
            doorCollider.isTrigger = true;
            newColor = new(mat.color.r, mat.color.g, mat.color.b, 0);
            isOpen = true;
        } else {
            doorCollider.isTrigger = false;
            newColor = new(mat.color.r, mat.color.g, mat.color.b, 1);
            isOpen = false;
        }
        mat.SetColor("_Color", newColor);
    }
}
