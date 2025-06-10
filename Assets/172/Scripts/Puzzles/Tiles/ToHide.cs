using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToHide : MonoBehaviour, IInteractable
{
    public bool hide = true;
    public GameObject grid;

    public void Interact()
    {
        hide = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!hide)
        {
            grid.SetActive(true);
        }
    }
    
    public void Highlight()
    {}

    public void Unhighlight()
    {}
}
