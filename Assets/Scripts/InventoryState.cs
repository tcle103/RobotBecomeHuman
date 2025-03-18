using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class InventoryState : MonoBehaviour
{
    private List<String> inventory;
    public bool inventoryOpen = false;
    [SerializeField] private GameObject inventoryUI;
    private Dictionary<String, Sprite> itemSprites;
    private List<GameObject> itemSlots;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<String>();
        itemSprites = new Dictionary<String, Sprite>();
        
        //load item sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>("Inventory/Icons");
        foreach (Sprite sprite in sprites)
        {
            itemSprites.Add(sprite.name, sprite);
            Debug.Log("Sprite added: " + sprite.name);
        }
        // inventory.Add("inventoryTest1");
        // inventory.Add("inventoryTest2");
        // inventory.Add("inventoryTest3");
        // inventory.Add("inventoryTest4");
        // inventory.Add("inventoryTest5");
        
        //get a list of all children(ui images) of inventory ui, these are the placeholder spots for items
        itemSlots = new List<GameObject>();
        foreach (Transform child in inventoryUI.transform)
        {
            itemSlots.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //on i key press, open or close inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            inventoryUI.SetActive(inventoryOpen);
        }
        
        //on esc key press, open main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
        }

        if (inventoryOpen)
        {
            for(int i = 0; i < itemSlots.Count; i++)
            {
                itemSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = null;
                itemSlots[i].SetActive(false);
            }
            
            Debug.Log("Inventory is open");
            foreach (String item in inventory)
            {
                Debug.Log("Item: " + item);
                
                Sprite sprite;
                if (itemSprites.TryGetValue(item, out sprite))
                {
                    Debug.Log("Sprite found");
                }
                else
                {
                    Debug.Log("Sprite not found");
                }
                
                for(int i = 0; i < itemSlots.Count; i++)
                {
                    if (itemSlots[i].GetComponent<UnityEngine.UI.Image>().sprite == null)
                    {
                        itemSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = sprite;
                        itemSlots[i].SetActive(true);
                        break;
                    }
                }
                
            }
        }
    }

    public void AddItem(string itemName)
    {
        inventory.Add(itemName);
    }
    
    public void RemoveItem(string itemName)
    {
        if (HasItem(itemName))
        {
            inventory.Remove(itemName);
        }
    }
    
    public bool HasItem(string itemName)
    {
        if (inventory.Contains(itemName))
        {
            return true;
        }
        return false;
    }
    
    public List<String> GetInventory()
    {
        return inventory;
    }

    public String SaveInventory()
    {
        //save inventory as a string - split by /
        String inventoryString = "";
        
        foreach (String item in inventory)
        {
            inventoryString += item + "/";
        }
        
        return inventoryString;
    }
    
    public void LoadInventory(String inventoryString)
    {
        //load inventory from string - split by /
        inventory.Clear();
        String[] items = inventoryString.Split('/');
        foreach (String item in items)
        {
            if (item != "")
            {
                inventory.Add(item);
            }
        }
    }
}
