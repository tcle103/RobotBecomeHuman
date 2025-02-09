using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetController : MonoBehaviour
{
    public Transform spawnZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() {
        foreach (Transform child in spawnZone.GetComponentsInChildren<Transform>()) {
            var controller = child.GetComponent<IGenericController>();
            if (controller != null) {
                controller.Reset();
            }
        }
    }
}
