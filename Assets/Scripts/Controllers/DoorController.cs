using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Color _openColor, _closedColor;
    SpriteRenderer _doorRenderer;
    Collider2D _doorCollider;
    bool isOpen;
    // Start is called before the first frame update
    void Awake()
    {
        _doorCollider = GetComponent<Collider2D>();
        _doorRenderer = GetComponent<SpriteRenderer>();
        _doorRenderer.color = _closedColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Operate() {
        if (!isOpen) {
            Open();
        } else {
            Close();
        }
    }

    public void Open() {
        _doorCollider.isTrigger = true;
        _doorRenderer.color = _openColor;
        isOpen = true;
    }

    public void Close() {
        _doorCollider.isTrigger = false;
        _doorRenderer.color = _closedColor;
        isOpen = false;
    }
}
