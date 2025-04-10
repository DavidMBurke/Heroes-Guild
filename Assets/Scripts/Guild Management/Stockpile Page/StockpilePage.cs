using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockpilePage : MonoBehaviour
{
    public GameObject itemList = null!;
    public GameObject itemListItemPrefab = null!;
    public Item selectedItem = null!;
    public TMP_InputField inputField = null!;
    public bool searchItemsByName = true;
    public bool searchItemsByTag = true;
    public GameObject searchByNameToggle = null!;
    public GameObject searchByTagToggle = null!;
    public ItemDisplayPanel itemDisplayPanel = null!;
    public GameObject noItemsText = null!;

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
        items = items.OrderBy(i => i.itemName).ThenBy(i => i.cost).ToList();
        foreach (Item item in items)
        {
            ItemListItem itemListItem = Instantiate(itemListItemPrefab, itemList.transform).GetComponent<ItemListItem>();
            itemListItem.SetItem(item);
            itemListItem.selectButton.onClick.AddListener(() => SelectItem(itemListItem.GetItem()));
        }
        noItemsText.gameObject.SetActive(items.Count == 0);
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
