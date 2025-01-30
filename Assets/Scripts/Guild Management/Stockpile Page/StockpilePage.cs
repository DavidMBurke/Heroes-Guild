using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockpilePage : MonoBehaviour
{
    public GameObject itemList;
    public GameObject itemListItemPrefab;
    public Item selectedItem;

    private void OnEnable()
    {
        ResetList();
    }

    void ResetList()
    {
        GuildManager.instance.stockpile.RemoveAll(item => item.quantity == 0);
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Item item in GuildManager.instance.stockpile)
        {
            ItemListItem itemListItem = Instantiate(itemListItemPrefab, itemList.transform).GetComponent<ItemListItem>();
            Button button = itemListItem.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectItem(item);
            });
            itemListItem.item = item;
        }
    }

    void SelectItem(Item item)
    {
        selectedItem = item;
    }
}
