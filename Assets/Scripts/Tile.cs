using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _offColor, _onColor;
    [SerializeField] private SpriteRenderer _tileRenderer;
    Collider2D _collider;
    List<Collider2D> _collisions;
    [SerializeField] List<GameObject> _neighbors;
    public bool isActivated;
    public bool isRoot;

    // Start is called before the first frame update
    void Start()
    {
        _tileRenderer.color = _offColor;
        _collider = GetComponent<Collider2D>();
        _collisions = new();
        FindNeighbors();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var collision in _collisions)
        {
            if (_collider.bounds.Contains(collision.bounds.center))
            {
                isRoot = true;
                if (!isActivated)
                    Activate();
                return;
            }
            else isRoot = false;
        }

        foreach (var neighbor in _neighbors)
        {
            Tile tile = neighbor.GetComponent<Tile>();
            if (tile.isRoot)
            {
                if (!isActivated)
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
        isRoot = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _collisions.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _collisions.Remove(other);
    }

    private void FindNeighbors()
    {
        _neighbors = new();

        Vector2[] directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(_collider.bounds.center, dir);
            if (hit && hit.collider.gameObject.GetComponent<Tile>())
            {
                Debug.Log("Hit " + hit.collider.gameObject);
                _neighbors.Add(hit.collider.gameObject);
            }
        }
    }
}
