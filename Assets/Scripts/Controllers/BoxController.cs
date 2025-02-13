using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour, IGenericController
{
    Vector3 originalPos;
    // Start is called before the first frame update
    void Awake()
    {
        originalPos = this.transform.position;
    }

    public void Activate() {}

    public void Reset() {
        this.transform.position = originalPos;
    }
}
