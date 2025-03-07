using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CraftingMaterialSlot : MonoBehaviour
{
    public Item? item;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public Sprite image;
    
    public void SetItem(Item? item = null)
    {
        this.item = item;
        itemName.text = item?.itemName?? "";
    }

    public bool CheckCorrectItemInSlot(List<string> tags)
    {
        if (tags.All(t => item.tags.Contains(t)))
        {
            return true;
        }
        return false;
    }

    public bool CheckCorrectItemQuantityInSlot(int requiredQuantity)
    {
        if (item.quantity >= requiredQuantity)
        {
            return true;
        }
        return false;
    }
}
