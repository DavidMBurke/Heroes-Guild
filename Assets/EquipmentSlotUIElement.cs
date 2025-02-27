using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUIElement : MonoBehaviour, IDropHandler
{
    public EquipmentSlots.Enum equipmentSlotEnum;
#nullable enable
    public Item? slotItem;
#nullable disable
    public TextMeshProUGUI itemName;


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryGridItemUIElement draggedItem))
        {
            Item newItem = draggedItem.GetItem();
            if (newItem == null || !newItem.equipSlots.Any(es => es == equipmentSlotEnum))
            {
                return;
            }
            PlayerCharacter character = CharacterInfoPanel.instance.character;
            EquipmentSlot slot = character.equipmentSlots.equipmentSlots[equipmentSlotEnum];
#nullable enable
            Item? replacedItem = slot.item;
#nullable disable
            if (draggedItem.source == InventorySource.Player)
            {
                character.RemoveFromInventory(newItem);
            }
            else if (draggedItem.source == InventorySource.Guild)
            {
                GuildManager.instance.stockpile.Remove(newItem);
            }
            else
            {
                Debug.Log("draggedItem has no source");
            }
            
            if (replacedItem != null)
            {
                if (draggedItem.source == InventorySource.Player)
                {
                    character.AddToInventory(slot.item);
                }
                if (draggedItem.source == InventorySource.Guild)
                {
                    GuildManager.instance.stockpile.Add(slot.item);
                }
            }

            draggedItem.transform.SetParent(null);
            Destroy(draggedItem.gameObject);
            character.equipmentSlots.EquipItem(equipmentSlotEnum, newItem);
            UpdateSlotItem(newItem);

            if (draggedItem.source == InventorySource.Player)
            {
                CharacterInfoPanel.instance.playerInventoryButtonClickHandler();
            }
            if (draggedItem.source == InventorySource.Guild)
            {
                CharacterInfoPanel.instance.guildInventoryButtonClickHandler();
            }

        }
    }

    public void UpdateSlotItem(Item item)
    {
        slotItem = item;
        itemName.text = item?.itemName ?? "";
    }

}
