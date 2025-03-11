using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActionAddItem : MonoBehaviour
{
    private InventoryState playerInventory;

    public string itemName;

    public void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryState>();
    }
    public void AddItem()
    {
        playerInventory.AddItem(itemName);
    }
}
