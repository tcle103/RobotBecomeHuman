using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _offColor, _onColor;
    [SerializeField] private SpriteRenderer _tileRenderer;
    AudioSource _audioSource;
    Collider2D _colliderSelf;
    Collider2D _colliderOther;
    [SerializeField] List<GameObject> _neighbors;
    public bool isActivated;
    public bool isRootTile;

    // Start is called before the first frame update
    void Start()
    {
        _tileRenderer.color = _offColor;
        _audioSource = GetComponent<AudioSource>();
        _colliderSelf = GetComponent<Collider2D>();
        FindNeighbors();
    }

    // Update is called once per frame
    void Update()
    {
        if (_colliderOther)
        {
            if (_colliderSelf.bounds.Contains(_colliderOther.bounds.center))
            {
                if (!isRootTile)
                {
                    _audioSource.Play();
                    isRootTile = true;
                }
                if (!isActivated) Activate();
                return;
            }
            isRootTile = false;
        }

        foreach (var neighbor in _neighbors)
        {
            Tile tile = neighbor.GetComponent<Tile>();
            if (tile.isRootTile)
            {
                if (!isActivated) Activate();
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
        isRootTile = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_colliderOther) _colliderOther = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_colliderOther == other) _colliderOther = null;
    }

    private void FindNeighbors()
    {
        _neighbors = new();

        Vector2[] directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(_colliderSelf.bounds.center, dir);
            if (hit && hit.collider.gameObject.GetComponent<Tile>())
            {
                Debug.Log("Hit " + hit.collider.gameObject);
                _neighbors.Add(hit.collider.gameObject);
            }
        }
    }
}
