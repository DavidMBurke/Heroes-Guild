using System.Collections;
using System.Collections.Generic;
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
        if (eventData.pointerDrag.TryGetComponent(out InventoryGridItem draggedItem))
        {
            var item = draggedItem.GetItem();
            if (item != null)
            {
                PlayerCharacter character = CharacterInfoPanel.instance.character;
                if (draggedItem.source == InventorySource.Player)
                {
                    character.RemoveFromInventory(item);
                    CharacterInfoPanel.instance.playerInventoryButtonClickHandler();
                }
                if (draggedItem.source == InventorySource.Guild)
                {
                    GuildManager.instance.stockpile.Remove(item);
                    CharacterInfoPanel.instance.guildInventoryButtonClickHandler();
                }
                character.equipmentSlots.EquipItem(equipmentSlotEnum, item);
                UpdateSlotItem(item);
            }
        }
    }

    public void UpdateSlotItem(Item item)
    {
        slotItem = item;
        itemName.text = item?.itemName ?? "";
    }

}
