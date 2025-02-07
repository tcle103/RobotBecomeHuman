using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTracker : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetState() {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
            if (child != this.transform && !child.gameObject.CompareTag("Respawn")) {
                Destroy(child.gameObject);
            }
        }
        Instantiate(prefab, gameObject.transform);
    }
}
