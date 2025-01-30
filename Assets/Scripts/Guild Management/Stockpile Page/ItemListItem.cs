using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemListItem : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;

    private void Start()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        itemName = textItems.First(tmp => tmp.gameObject.name == "Name");
        itemQuantity = textItems.First(tmp => tmp.gameObject.name == "Quantity");
    }

    private void Update()
    {
        if (item == null)
        {
            Debug.LogError("No item assigned to ItemListItem");
        }
        itemName.text = item.itemName;
        itemQuantity.text = item.quantity.ToString();
    }
}
