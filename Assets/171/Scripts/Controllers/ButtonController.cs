using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour, IGenericController
{
    [SerializeField] private Color _onColor, _offColor;
    SpriteRenderer _buttonRenderer;
    public bool isOn;

    void Awake() {
        _buttonRenderer = GetComponent<SpriteRenderer>();
        _buttonRenderer.color = _offColor;
    }
    public void Toggle() {
        if (!isOn)
            Activate();
        else
            Deactivate();
    }
    public void Activate() {
        _buttonRenderer.color = _onColor;
        isOn = true;
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
