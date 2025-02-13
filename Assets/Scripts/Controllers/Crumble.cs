using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crumble : MonoBehaviour
{
    public int crumbleTime = 50;
    public float crumbleRate = 0.01f;
    float tileOpacity = 0;
    bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && tileOpacity > 0) {
            tileOpacity -= crumbleRate;
            var mat = gameObject.GetComponent<Renderer>().material;
            Color oldColor = mat.color;
            Color newColor = new(oldColor.r, oldColor.g, oldColor.b, tileOpacity);
            mat.SetColor("_Color", newColor);
        }
    }
    public void Activate() {
        if (!triggered) {
            tileOpacity = gameObject.GetComponent<Renderer>().material.color.a;
            Debug.Log(tileOpacity);
            triggered = true;
        }
    }
}
