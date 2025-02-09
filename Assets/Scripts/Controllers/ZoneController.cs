using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    bool isComplete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        if (!isComplete) {
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
                var controller = child.GetComponent<IGenericController>();
                controller?.Reset();
            }
        }
    }

    public void Validate() {
        isComplete = true;
    }
}
