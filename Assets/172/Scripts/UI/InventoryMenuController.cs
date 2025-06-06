/*
 * Last modified by: Dale Spence
 * Last modified on: 5 / 15 / 25
 *
 * Inventory Menu Controller handles navigating and managing the player inventory
*/
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryMenuController : MonoBehaviour
{
    public CanvasGroup inventoryCanvasGroup;
    public List<TextMeshProUGUI> itemLabels;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailDescription;
    public PlayerController playerMovementScript;
    public PlayerData playerData;
    public bool gameIsPaused = false;
    public ItemDict itemDatabase; // reference to ItemDict ScriptableObject

    private List<string> inventoryItems; // Replace inner ItemData class

    private int selectedIndex = 0;
    private bool isOpen = false;
    
    void Update()
    {
        if (gameIsPaused) return; // block all inventory input if paused

        if (Keyboard.current.pKey.wasPressedThisFrame || ((Gamepad.current != null) && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            AddItemByID("testItem");
            Debug.Log("Test item added.");
        }

        if (Keyboard.current.iKey.wasPressedThisFrame || ((Gamepad.current != null) && Gamepad.current.selectButton.wasPressedThisFrame))
        {
            ToggleInventory();
        }

        if (!isOpen) return;

        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateInventoryDisplay();
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            selectedIndex = Mathf.Min(inventoryItems.Count - 1, selectedIndex + 1);
            UpdateInventoryDisplay();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
        {
            //UseSelectedItem();
            Debug.Log("Using item: [not implemented]");
        }
        
        // controller/gamepad inputs
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame)
            {
                selectedIndex = Mathf.Max(0, selectedIndex - 1);
                UpdateInventoryDisplay();
            }
            else if (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame)
            {
                selectedIndex = Mathf.Min(inventoryItems.Count - 1, selectedIndex + 1);
                UpdateInventoryDisplay();
            }

            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                //UseSelectedItem();
                Debug.Log("Using item: [not implemented]");
            }
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;

        inventoryCanvasGroup.alpha = isOpen ? 1 : 0;
        inventoryCanvasGroup.interactable = isOpen;
        inventoryCanvasGroup.blocksRaycasts = isOpen;

        if (playerMovementScript != null)
            playerMovementScript.enabled = !isOpen;

        if (isOpen)
        {
            selectedIndex = 0;
            UpdateInventoryDisplay();
        }
    }

    void UpdateInventoryDisplay()
    {
        inventoryItems = new List<string>(playerData.inventoryGet().Keys);
        for (int i = 0; i < itemLabels.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                string label = itemDatabase.List[inventoryItems[i]].itemName; // Uses ScriptableObject itemName
                itemLabels[i].text = (i == selectedIndex ? "> " : "  ") + label;
            }
            else
            {
                itemLabels[i].text = "";
            }
        }

        if (selectedIndex < inventoryItems.Count)
        {
            detailName.text = itemDatabase.List[inventoryItems[selectedIndex]].itemName; // Uses ScriptableObject itemName
            detailDescription.text = itemDatabase.List[inventoryItems[selectedIndex]].description;
        }
        else
        {
            detailName.text = "";
            detailDescription.text = "";
        }
    }

    public void ForceCloseInventory()
    {
        isOpen = false;
        inventoryCanvasGroup.alpha = 0;
        inventoryCanvasGroup.interactable = false;
        inventoryCanvasGroup.blocksRaycasts = false;
    }

    // Add item by ID using the ItemDict database
    public void AddItemByID(string id)
    {
        if (itemDatabase != null && itemDatabase.List.ContainsKey(id))
        {
            inventoryItems.Add(id);

            if (isOpen)
            {
                UpdateInventoryDisplay();
            }
        }
        else
        {
            Debug.LogWarning("Item ID not found in ItemDict: " + id);
        }
    }
}