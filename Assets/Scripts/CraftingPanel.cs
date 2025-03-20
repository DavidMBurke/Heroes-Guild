using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    public GameObject materialSlotsParent = null!;
    List<CraftingMaterialSlot> materialSlots = null!;
    public CraftingMaterialSlot selectedSlot = null!;
    public List<string> selectedTags = null!;

    public GameObject itemListItemPrefab = null!;
    public GameObject materialList = null!;
    public Button addToQueueButton = null!;
    public TextMeshProUGUI craftingMenuHeader = null!;

    public WorkshopPage workshopPage = null!;
    public CraftingOption selectedCraftingOption = null!;
    private List<PlayerCharacter> crafters = null!;

    GuildManager gm = null!;

    private void Start()
    {
        gm = GuildManager.instance;
        materialSlots = materialSlotsParent.GetComponentsInChildren<CraftingMaterialSlot>().ToList();
        SelectCraftingOption(selectedCraftingOption);
    }

    public void InitializeCraftingOptions(List<PlayerCharacter> crafterList)
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
        selectedTags = tags;
        selectedSlot = itemSlot;
        List<Item> items = new List<Item>();

        foreach (Item item in gm.stockpile)
        {
            if (item.tags.Any((t) => tags.Contains(t)))
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

        // Create the crafted item
        Item craftedItem = selectedCraftingOption.craftingFunction(materials);
        if (craftedItem == null) {
            Debug.Log($"No item returned for {selectedCraftingOption}");
            return;
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

        // Add to crafting queue
        GameObject craftedObject = new GameObject();
        ItemInQueue itemInQueue = craftedObject.AddComponent<ItemInQueue>();
        itemInQueue.SetNewItem(craftedItem, crafters, workshopPage.craftingQueue.itemQueue);
        craftedObject.name = craftedItem.itemName;
        workshopPage.craftingQueue.itemQueue.Add(itemInQueue);
        workshopPage.craftingQueue.UpdateQueueList();
        SelectItemSlot(selectedTags, selectedSlot);
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
