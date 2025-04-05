/* 
 * Last modified by: Tien Le
 * Last modified on: 4/5/25
 * 
 * PlayerData.cs contains all player-specific data
 * that needs to be maintained in a continuous session as well as
 * across application closure/reboot. Additionally, PlayerData.cs
 * should expose and safely handle modification to any data that
 * that may need to in order to reflect player state.
 * Should include: inventory
 * 
 * Created by: Tien Le
 * Created on: 3/28/25
 * Contributers: Tien Le 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerData : MonoBehaviour
{   
    // [3/29/25 Tien] 
    // inventory holds string-int pairs representing
    // the name of the item (corresponding to the item dict entry)
    // and the amount of that item in the inventory
    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    // [3/29/25 Tien] reference to the master list of 
    // item labels to item data
    [SerializeField] private ItemDict itemMasterList;
    // [4/5/25 Tien] a test property
    public bool Yes { get; set; } = false;


    // Start is called before the first frame update
    void Start()
    {
        //inventoryAdd("testItem");
    }

    // [3/29/25 Tien] does what it says it does
    public Dictionary<string, int> inventoryGet()
    {
        return inventory;
    }

    // [3/29/25 Tien] adds new items to inventory, otherwise
    // increases count of existing item by amount
    public void inventoryAdd(string itemLabel, int amount = 1)
    {
        // [3/29/25 Tien] checks if label refers to valid item
        if (itemMasterList.List.ContainsKey(itemLabel))
        {
            if (inventory.ContainsKey(itemLabel))
            {
                inventory[itemLabel] += amount;
            }
            else
            {
                inventory.Add(itemLabel, amount);
            }
        }
        else
        {
            Debug.LogWarning("ERROR: tried to add invalid item label '" + itemLabel + "' to inventory, check the master list", this);
        }
    }
    
    // [3/29/25 Tien] decreases number of item in inventory by amount,
    // removing item as needed
    public void inventoryRemove(string itemLabel, int amount = 1) 
    {
        if (inventory.ContainsKey(itemLabel))
        {
            if (inventory[itemLabel] >= amount)
            {
                inventory[itemLabel] -= amount;
                if (inventory[itemLabel]<= 0)
                {
                    inventory.Remove(itemLabel);
                }
            }
            else
            {
                Debug.LogWarning("ERROR: Did not remove item '" + itemLabel + "', not enough of item in inventory", this);
            }
        }
        else
        {
            Debug.LogWarning("ERROR: Did not remove item '" + itemLabel + "', item not found in inventory", this);
        }
    }


}
