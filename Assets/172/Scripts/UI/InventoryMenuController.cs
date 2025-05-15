
/*Last modified by: Dale Spence
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
    public bool gameIsPaused = false;

    private int selectedIndex = 0;
    private bool isOpen = false;

    [System.Serializable]
    public class ItemData
    {
        public string name;
        public string description;
    }

    public List<ItemData> inventoryItems;

    void Update()
    {
        if (gameIsPaused) return; // block all inventory input if paused

        if (Keyboard.current.iKey.wasPressedThisFrame)
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
        for (int i = 0; i < itemLabels.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                string label = inventoryItems[i].name;
                itemLabels[i].text = (i == selectedIndex ? "> " : "  ") + label;
            }
            else
            {
                itemLabels[i].text = "";
            }
        }

        if (selectedIndex < inventoryItems.Count)
        {
            detailName.text = inventoryItems[selectedIndex].name;
            detailDescription.text = inventoryItems[selectedIndex].description;
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

    /*
    public void AddItem(string name, string description)
    {
        inventoryItems.Add(new ItemData { name = name, description = description });

        if (isOpen)
        {
            UpdateInventoryDisplay();
        }
    }
    */
}