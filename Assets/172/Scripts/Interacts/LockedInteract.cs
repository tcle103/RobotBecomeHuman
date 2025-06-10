using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockedInteract : MonoBehaviour, IInteractable
{
    private PlayerData playerData;
    [SerializeField] private string keyItem;
    public UnityEvent action;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Dictionary<string, int> currentInventory = playerData.inventoryGet();
        if (currentInventory.ContainsKey(keyItem) || keyItem == "none")
        {
            action.Invoke();
        }
    }

    public void Highlight()
    {}

    public void Unhighlight()
    {}
}
