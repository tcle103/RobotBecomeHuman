using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Respawn : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnZone;
    public AudioClip grunt;
    AudioSource _audioSource;
    InputAction respawnAction;
    // Start is called before the first frame update
    void Start()
    {
        respawnAction = InputSystem.actions.FindAction("Respawn");
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnPoint && respawnAction.triggered) {
            this.transform.position = new (spawnPoint.position.x, spawnPoint.position.y);
            
            if (spawnZone) {
                spawnZone.GetComponent<ZoneController>().Reset();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Death")) {
            this.transform.position = new (spawnPoint.position.x, spawnPoint.position.y);
            _audioSource.PlayOneShot(grunt, 0.7f);
            if (spawnZone) {
                spawnZone.GetComponent<ZoneController>().Reset();
            }
        }
        if (collision.transform.CompareTag("Zone")) {
            spawnZone = collision.transform;
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
