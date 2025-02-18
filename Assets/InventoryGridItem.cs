using TMPro;
using UnityEngine;

public class InventoryGridItem : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    Item item;

    public void SetItem(Item item)
    {
        this.item = item;
        itemName.text = item.itemName;
        itemQuantity.text = item.quantity.ToString();
    }
}
