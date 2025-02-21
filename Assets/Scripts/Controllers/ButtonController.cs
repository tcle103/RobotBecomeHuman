using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour, IGenericController
{
    [SerializeField] private Color _onColor, _offColor;
    SpriteRenderer _buttonRenderer;
    public bool isOn;
    // bool isActive;

    // public void Press() {
    //     if (isActive) {
    //         if (!isOn) {
    //             gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    //             isOn = true;
    //         } else {
    //             Reset();
    //         }
    //         Debug.Log("Activated");
    //     }
    // }

    void Awake() {
        _buttonRenderer = GetComponent<SpriteRenderer>();
        _buttonRenderer.color = _offColor;
    }
    public void Activate() {
        if (!isOn) {
            _buttonRenderer.color = _onColor;
            isOn = true;
        }
    }
    public void Reset() {
        _buttonRenderer.color = _offColor;
        isOn = false;
    }
    public void Deactivate() {
        if (isOn)
            Reset();
    }
}
