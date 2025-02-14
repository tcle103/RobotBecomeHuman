using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _offColor, _onColor;
    [SerializeField] private SpriteRenderer _tileRenderer;
    Collider2D _collider;
    List<Collider2D> _collisions;
    public bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
        _tileRenderer.color = _offColor;
        _collider = GetComponent<Collider2D>();
        _collisions = new();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var collision in _collisions)
        {
            if (_collider.bounds.Contains(collision.bounds.center))
            {
                Activate();
                return;
            }
        }
        Deactivate();
    }

    public void Activate()
    {
        _tileRenderer.color = _onColor;
        isActivated = true;
    }

    public void Deactivate()
    {
        _tileRenderer.color = _offColor;
        isActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _collisions.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        _collisions.Remove(other);
    }
}
