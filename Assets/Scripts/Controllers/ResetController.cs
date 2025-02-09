using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetController : MonoBehaviour
{
    public Transform spawnZone;
    // Start is called before the first frame update

    public void Activate() {
        foreach (Transform child in spawnZone.GetComponentsInChildren<Transform>()) {
            var controller = child.GetComponent<IGenericController>();
            controller?.Reset();
        }
    }
}
