using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LockInteract : MonoBehaviour, IInteractable
{
    private InventoryState inventory;
    [SerializeField] private string keyItem;
    public UnityEvent action;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (inventory && inventory.HasItem(keyItem))
        {
            action.Invoke();
        }
    }

    public void Highlight()
    {}

    public void Unhighlight()
    {}
}
