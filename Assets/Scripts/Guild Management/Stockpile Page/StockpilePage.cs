using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StockpilePage : MonoBehaviour
{
    public GameObject itemList;
    public GameObject itemListItemPrefab;
    public Item selectedItem;
    public TMP_InputField inputField;
    public bool searchItemsByName = true;
    public bool searchItemsByTag = true;
    public GameObject searchByNameToggle;
    public GameObject searchByTagToggle;
    public ItemDisplayPanel itemDisplayPanel;

    private void OnEnable()
    {
        ResetList();
    }

    void ResetList()
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
        if (inputField.text == "")
        {
            items = GuildManager.instance.stockpile;
        }
        if (inputField.text != "")
        {
            items = GuildManager.instance.stockpile.Where(i => CheckNameFit(i, inputField.text, searchItemsByName, searchItemsByTag)).ToList();
        }
        foreach (Item item in items)
        {
            ItemListItem itemListItem = Instantiate(itemListItemPrefab, itemList.transform).GetComponent<ItemListItem>();
            itemListItem.item = item;
            itemListItem.selectButton.onClick.AddListener(() => SelectItem(itemListItem.item));
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
        ResetList();
    }

    public void SelectItem(Item item)
    {
        Debug.Log("Select Item");
        selectedItem = item;
        itemDisplayPanel.AssignItem(selectedItem);
    }

    public void ToggleSearchByName()
    {
        searchItemsByName = !searchItemsByName;
        ToggleHighlight(searchByNameToggle, searchItemsByName);
        ResetList();
    }

    public void ToggleSearchByTag()
    {
        searchItemsByTag = !searchItemsByTag;
        ToggleHighlight(searchByTagToggle, searchItemsByTag);
        ResetList();
    }

    public void ToggleHighlight(GameObject gameObject, bool toggle)
    {
        Color c = gameObject.GetComponent<Image>().color;
        Color newColor = new Color(c.r, c.g, c.b, toggle ? 1f : .5f);
        gameObject.GetComponent<Image>().color = newColor;
    }
}
