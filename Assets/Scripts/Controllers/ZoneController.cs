using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    bool isActive;
    bool isComplete;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open() {
        isActive = true;
        foreach (Transform child in GetComponentsInChildren<Transform>()) {
            if (child.gameObject.CompareTag("Gate")) {
                Destroy(child.gameObject);
            }
            var controller = child.GetComponent<IGenericController>();
            controller?.Activate();
        }
    }

    public void Reset() {
        if (isActive) {
            foreach (Transform child in GetComponentsInChildren<Transform>()) {
                var controller = child.GetComponent<IGenericController>();
                controller?.Reset();
            }
        }
    }

    public void Validate() {
        isActive = false;
        isComplete = true;
    }
}
