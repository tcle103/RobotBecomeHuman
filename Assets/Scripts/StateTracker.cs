using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateTracker : MonoBehaviour
{
    public GameObject prefab;
    bool resetLive;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            resetLive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ResetState();
            resetLive = false;
        }
    }

    void ResetState() {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
            if (child != this.transform) {
                Destroy(child.gameObject);
            }
        }
        Instantiate(prefab, gameObject.transform);
    }
}
