using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    public CraftingMaterialSlot itemSlot1;
    public CraftingMaterialSlot itemSlot2;
    CraftingMaterialSlot selectedSlot;

    public GameObject materialList;
    public Button addToQueueButton;
    public TextMeshProUGUI craftingMenuHeader;

    public WorkshopPage workshopPage;

    GuildManager gm;

    private void Start()
    {
        gm = GuildManager.instance;
    }

    public void SelectIngotSlot()
    {
        List<string> tags = new List<string>()
        {
            "metal",
            "jewelry"
        };
        SelectItemSlot(tags, itemSlot1);
    }

    public void SelectGemSlot()
    {
        List<string> tags = new List<string>()
        {
            "gem"
        };
        SelectItemSlot(tags, itemSlot2);
    }

    public void SelectItemSlot(List<string> tags, CraftingMaterialSlot itemSlot)
    {
        List<Item> items = new List<Item>();

        foreach (Item item in gm.stockpile)
        {
            if (item.tags.All((t) => tags.Contains(t)))
            {
                items.Add(item);
            };
        }

        workshopPage.UpdateItemList(items, itemSlot);
        selectedSlot = itemSlot;
    }

    public void AddNecklaceToQueue()
    {
        if (itemSlot1.item == null || itemSlot2.item == null)
        {
            return;
        }
        List<string> tags1 = new List<string> { "metal", "jewelry" };
        List<string> tags2 = new List<string> { "gem" };
        if (itemSlot1.CheckCorrectItemInSlot(tags1) == false || itemSlot2.CheckCorrectItemInSlot(tags2) == false)
        {
            Debug.Log("Correct Items not in slots");
            return;
        }
        if (itemSlot1.CheckCorrectItemQuantityInSlot(1) == false || itemSlot2.CheckCorrectItemQuantityInSlot(1) == false)
        {
            Debug.Log("Insufficient Quantities in slots");
            return;
        }
        itemSlot1.item.quantity -= 1;
        itemSlot2.item.quantity -= 1;
        Item necklace = Jewelry.CreateNecklace(itemSlot1.item, itemSlot2.item);
        GameObject necklaceObject = new GameObject();
        ItemInQueue necklaceInQueue = necklaceObject.AddComponent<ItemInQueue>();
        necklaceInQueue.SetNewItem(necklace, gm.jewelers, workshopPage.itemQueue);
        necklaceObject.name = necklace.itemName;
        workshopPage.itemQueue.Add(necklaceInQueue);
        workshopPage.UpdateQueueList();
        SelectItemSlot(selectedSlot.item.tags, selectedSlot);
    }
}
