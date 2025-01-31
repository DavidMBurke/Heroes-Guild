using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListItem : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI quantity;
    public Button selectButton;


    private void Awake()
    {
        selectButton = GetComponent<Button>();
    }
    private void Start()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        itemName = textItems.First(tmp => tmp.gameObject.name == "Name");
        quantity = textItems.First(tmp => tmp.gameObject.name == "Quantity");
    }

    private void Update()
    {
        if (item == null)
        {
            Debug.LogError("No item assigned to ItemListItem");
        }
        itemName.text = item.itemName;
        quantity.text = item.quantity.ToString();
    }

}
