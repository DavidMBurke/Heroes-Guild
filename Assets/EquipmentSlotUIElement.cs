using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentSlotUIElement : MonoBehaviour
{
    public EquipmentSlots.Enum equipmentSlotEnum;
#nullable enable
    public Item? slotItem;
#nullable disable
    public TextMeshProUGUI itemName;

    public void UpdateSlotItem(Item item)
    {
        slotItem = item;
        itemName.text = item.itemName;
    }

}
