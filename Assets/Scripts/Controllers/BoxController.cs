using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour, IGenericController
{
    Vector3 originalPos;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        originalPos = this.transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    public void Activate() {
        rb.isKinematic = false;
    }

    public void Reset() {
        this.transform.position = originalPos;
    }

    public void Deactivate() {
        rb.isKinematic = true;
    }
}
