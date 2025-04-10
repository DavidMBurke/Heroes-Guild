#nullable enable
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMaterialSlot : MonoBehaviour
{
    public Item? item;
    public TextMeshProUGUI itemName = null!;
    public TextMeshProUGUI itemType = null!;
    public Image image = null!;
    
    public void SetItem(Item? item = null)
    {
        this.item = item;
        if (item == null) {
            itemName.text = string.Empty;
            if (image?.sprite != null)
            {
                image.sprite = null;
            }
            return;
        }
        itemName.text = item.itemName;
        if (image?.sprite != null) //bandaid for playest, getting null reference errors here
        {
            image.sprite = item.sprite ?? null;
        }
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
        return item != null && item.quantity >= requiredQuantity;
    }

}
