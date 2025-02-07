using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTracker : MonoBehaviour
{
    public float visibilityRate = 0.01f;
    float childOpacity = 0;
    bool reveal;

    // Start is called before the first frame update
    void Start()
    {
        SetChildrenAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (reveal && childOpacity < 1) {
            childOpacity += visibilityRate;
            SetChildrenAlpha(childOpacity);
        } else if (!reveal && childOpacity > 0) {
            childOpacity -= visibilityRate;
            SetChildrenAlpha(childOpacity);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            reveal = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            reveal = false;
        }
    }

    void SetChildrenAlpha(float alpha) {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
            if (child != this.transform) {
                var mat = child.GetComponent<Renderer>().material;
                Color newColor = new(mat.color.r, mat.color.g, mat.color.b, alpha);
                mat.SetColor("_Color", newColor);
            }
        }
    }
}
