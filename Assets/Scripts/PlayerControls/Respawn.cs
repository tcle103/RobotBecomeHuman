using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Respawn : MonoBehaviour
{
    public Transform spawnPoint;
    InputAction respawnAction;
    // Start is called before the first frame update
    void Start()
    {
        respawnAction = InputSystem.actions.FindAction("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnPoint && respawnAction.triggered) {
            this.transform.position = spawnPoint.position;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Death")) {
            this.transform.position = spawnPoint.position;
        }
        if (collision.transform.CompareTag("Zone")) {
            spawnPoint = GetChildWithTag(collision.gameObject, "Respawn");
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.CompareTag("Zone")) {
            spawnPoint = null;
        }
    }

    Transform GetChildWithTag(GameObject parent, string tag) {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>()) {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }
}
