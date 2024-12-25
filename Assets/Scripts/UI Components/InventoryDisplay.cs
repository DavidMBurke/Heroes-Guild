using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public PlayerCharacter currentPlayer;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    void Start()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>().ToList();
        ActionManager.instance.OnPlayerSelected += HandlePlayerSelected;
        UpdateDisplay();
    }

    private void OnDestroy()
    {
        ActionManager.instance.OnPlayerSelected -= HandlePlayerSelected;
        if (currentPlayer != null)
        {
            currentPlayer.OnInventoryUpdated -= UpdateDisplay;
        }
    }

    void HandlePlayerSelected(PlayerCharacter newPlayer)
    {
        if (currentPlayer != null)
        {
            currentPlayer.OnInventoryUpdated -= UpdateDisplay;
        }

        currentPlayer = newPlayer;
        currentPlayer.OnInventoryUpdated += UpdateDisplay;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (currentPlayer == null)
        {
            return;
        }


        foreach ((InventorySlot inventorySlot, int i) in inventorySlots.Select((s, i) => (s, i)))
        {
            if (i < currentPlayer.inventory.Count)
            {
                inventorySlot.UpdateItem(currentPlayer.inventory[i]);
                continue;
            }
            inventorySlot.UpdateItem(null);
        }
    }
}
