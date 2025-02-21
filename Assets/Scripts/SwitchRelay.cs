using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchRelay : MonoBehaviour
{
    [SerializeField] private List<GameObject> _switches;
    List<ButtonController> _controllers;
    public UnityEvent action;
    bool switched;
    // Start is called before the first frame update
    void Awake()
    {
        _controllers = new();
        foreach (var obj in _switches) {
            ButtonController controller = obj.GetComponent<ButtonController>();
            if (controller) _controllers.Add(controller);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_controllers.Count > 0 && !switched) {
            foreach (var controller in _controllers) {
                if (!controller.isOn)
                    return;
            }
            action.Invoke();
            switched = true;
        }
    }
}
