using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Respawn : MonoBehaviour
{
    public GameObject spawnPoint;

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
            this.transform.position = spawnPoint.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Death") {
            this.transform.position = spawnPoint.transform.position;
        }
    }
}
