using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteract: MonoBehaviour, IInteractable
{
    public UnityEvent action;
    AudioSource _audioSource;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        action.Invoke();
        _audioSource.Play();
    }

    public void Highlight()
    {}

    public void Unhighlight()
    {}
}
