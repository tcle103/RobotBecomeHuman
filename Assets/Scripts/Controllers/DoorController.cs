using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int id;

    [SerializeField] private Color _openColor, _closedColor;
    SpriteRenderer _doorRenderer;
    Collider2D _doorCollider;
    AudioSource _audioSource;
    bool _isOpen;

    // Start is called before the first frame update
    void Awake()
    {
        _doorCollider = GetComponent<Collider2D>();
        _doorRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _doorRenderer.color = _closedColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Operate() {
        if (!_isOpen) {
            Open();
        } else {
            Close();
        }
    }

    public void OpenQuiet()
    {
        _doorCollider.isTrigger = true;
        _doorRenderer.color = _openColor;
        _isOpen = true;
    }

    public void Open() {
        if (!_isOpen) _audioSource.Play();
        OpenQuiet();
    }

    public bool isOpen()
    {
        return _isOpen;
    }

    public void Close() {
        _doorCollider.isTrigger = false;
        _doorRenderer.color = _closedColor;
        if (_isOpen) _audioSource.Play();
        _isOpen = false;
    }
}
