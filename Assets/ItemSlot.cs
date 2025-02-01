using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI itemName;
    public Sprite image;
    
    public void SetItem(Item item)
    {
        this.item = item;
        itemName.text = item.itemName;
    }
}
