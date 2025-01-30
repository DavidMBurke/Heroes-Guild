using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockpilePage : MonoBehaviour
{
    public GameObject itemList;
    public GameObject itemListItemPrefab;
    public Item selectedItem;
    public TMP_InputField inputField;
    public bool searchItemsByName = true;
    public bool searchItemsByTag = true;

    private void OnEnable()
    {
        ResetList();
    }

    void ResetList(string searchText = "", bool searchByName = false, bool searchByTag = false)
    {
        if (GuildManager.instance == null || GuildManager.instance.stockpile == null)
        {
            return;
        }
        GuildManager.instance.stockpile.RemoveAll(item => item.quantity == 0);
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        List<Item> items = new List<Item>();
        if (searchText == "")
        {
            items = GuildManager.instance.stockpile;
        }
        if (searchText != "")
        {
            items = GuildManager.instance.stockpile.Where(i => CheckNameFit(i, searchText, searchByName, searchByTag)).ToList();
        }
        foreach (Item item in items)
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

    private bool CheckNameFit(Item item, string searchText, bool searchByName, bool searchByTag)
    {
        string search = searchText.ToLower();
        if (searchByName)
        {
            if (item.itemName.ToLower().Contains(search)) {
                return true;
            }
        }
        if (searchByTag)
        {
            if (item.tags.Any(t => t.ToLower().Contains(search)))
            {
                return true;
            }
        }
        return false;
    }

    public void HandleTextFieldEntry()
    {
        if (inputField.text == "")
        {
            ResetList();
        }
        ResetList(inputField.text, searchItemsByName, searchItemsByTag);
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;
    }
}
