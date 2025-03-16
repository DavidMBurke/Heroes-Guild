using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    public GameObject materialSlotsParent;
    List<CraftingMaterialSlot> materialSlots;

    public GameObject itemListItemPrefab;
    public GameObject materialList;
    public Button addToQueueButton;
    public TextMeshProUGUI craftingMenuHeader;

    public WorkshopPage workshopPage;
    public CraftingOption selectedCraftingOption;
    private List<PlayerCharacter> crafters;

    GuildManager gm;

    private void Start()
    {
        gm = GuildManager.instance;
        materialSlots = materialSlotsParent.GetComponentsInChildren<CraftingMaterialSlot>().ToList();
    }

    public void InitializeCraftingOptions(List<CraftingOption> craftingOptions, List<PlayerCharacter> crafterList)
    {
        crafters = crafterList;

    }

    public void SelectCraftingOption(CraftingOption option)
    {
        selectedCraftingOption = option;
        craftingMenuHeader.text = $"Craft {option.itemName}";
        SetupMaterialSlots(option);
    }

    public void SetupMaterialSlots(CraftingOption option)
    {
        if (materialSlots == null || materialSlots.Count == 0)
        {
            materialSlots = materialSlotsParent.GetComponentsInChildren<CraftingMaterialSlot>().ToList();
        }

        for (int i = 0; i < materialSlots.Count(); i++)
        {
            if (i < option.requiredMaterials.Count)
            {
                materialSlots[i].gameObject.SetActive(true);
                materialSlots[i].itemType.text = $"{option.requiredMaterials[i].itemTypeName} (x{option.requiredMaterials[i].quantity})";
                materialSlots[i].SetItem(null);
                int index = i;
                Button slotButton = materialSlots[i].GetComponentInChildren<Button>();
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => SelectItemSlot(option.requiredMaterials[index].tags, materialSlots[index]));
            }
            else
            {
                materialSlots[i].gameObject.SetActive(false);
            }
        }
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

        UpdateItemList(items, itemSlot);
    }

    public void AddItemToQueue()
    {
        if (selectedCraftingOption == null) return;

        List<Item> materials = new List<Item>();

        // Ensure all required materials are available in sufficient quantity
        for (int i = 0; i < selectedCraftingOption.requiredMaterials.Count; i++)
        {
            var requiredMaterial = selectedCraftingOption.requiredMaterials[i];
            var slot = materialSlots[i];

            if (slot.item == null || slot.item.quantity < requiredMaterial.quantity)
            {
                Debug.Log($"Insufficient {requiredMaterial.itemTypeName}. Required: {requiredMaterial.quantity}, Available: {slot.item?.quantity ?? 0}");
                return;
            }
            materials.Add(slot.item);
        }

        // Deduct the correct amount from each material
        foreach (var requiredMaterial in selectedCraftingOption.requiredMaterials)
        {
            // Find the corresponding item in the materials list
            var materialItem = materials.FirstOrDefault(item => item.tags.Any(tag => requiredMaterial.tags.Contains(tag)));

            if (materialItem != null)
            {
                materialItem.quantity -= requiredMaterial.quantity;
            }
        }

        // Create the crafted item
        Item craftedItem = selectedCraftingOption.craftingFunction(materials);

        // Add to crafting queue
        GameObject craftedObject = new GameObject();
        ItemInQueue itemInQueue = craftedObject.AddComponent<ItemInQueue>();
        itemInQueue.SetNewItem(craftedItem, crafters, workshopPage.craftingQueue.itemQueue);
        craftedObject.name = craftedItem.itemName;
        workshopPage.craftingQueue.itemQueue.Add(itemInQueue);
        workshopPage.craftingQueue.UpdateQueueList();
    }


    public void UpdateItemList(List<Item> items, CraftingMaterialSlot itemSlot)
    {
        items.RemoveAll(i => i.quantity == 0);
        foreach (Transform child in materialList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Item item in items)
        {
            ItemListItem itemListItem = Instantiate(itemListItemPrefab, materialList.transform).GetComponent<ItemListItem>();
            itemListItem.SetItem(item);
            itemListItem.selectButton.onClick.AddListener(() => itemSlot.SetItem(item));
        }
    }
}
