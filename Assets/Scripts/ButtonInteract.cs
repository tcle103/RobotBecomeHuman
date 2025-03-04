using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteract: MonoBehaviour, IInteractable
{
    public UnityEvent action;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        action.Invoke();
    }

    public void Highlight()
    {}

    public void Unhighlight()
    {}
}
