using TMPro;
using UnityEngine;

public class ItemDisplayPanel : MonoBehaviour
{
    Item item;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI quantity;

    public void AssignItem(Item item)
    {
        this.item = item;
        Debug.Log("Assign item");
        UpdateItem();
    }

    public void UpdateItem()
    {
        Debug.Log("Update Item");
        itemName.text = item.itemName;
        description.text = item.description;
        quantity.text = item.quantity.ToString();
    }
}
