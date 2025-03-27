using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour, IGenericController
{
    Vector3 _originalPos;
    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Awake()
    {
        _originalPos = this.transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Zone"))
            Reset();
    }

    public void Activate()
    {
        _rb.isKinematic = false;
    }

    public void Reset()
    {
        this.transform.position = _originalPos;
    }

    public void Deactivate()
    {
        _rb.isKinematic = true;
    }
}
