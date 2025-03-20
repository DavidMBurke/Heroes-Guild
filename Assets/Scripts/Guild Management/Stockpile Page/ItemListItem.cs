using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListItem : MonoBehaviour
{
    private Item item = null!;
    private TextMeshProUGUI itemName = null!;
    private TextMeshProUGUI quantity = null!;
    public Image image = null!;
    public Button selectButton = null!;


    private void Awake()
    {
        selectButton = GetComponent<Button>();
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        itemName = textItems.First(tmp => tmp.gameObject.name == "Name");
        quantity = textItems.First(tmp => tmp.gameObject.name == "Quantity");
    }

    public void SetItem(Item newItem)
    {
        item = newItem; 
        UpdateDisplayInfo();
    }

    private void UpdateDisplayInfo()
    {
        if (item == null)
        {
            Debug.LogError("No Item assigned to ItemListItem");
        }
        itemName.text = item.itemName;
        quantity.text = item.quantity.ToString();
        image.sprite = item.sprite;
    }

    public Item GetItem()
    {
        return item;
    }
}
