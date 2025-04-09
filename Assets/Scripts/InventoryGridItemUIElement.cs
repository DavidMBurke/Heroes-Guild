#nullable enable
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGridItemUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    CanvasGroup canvasGroup = null!;
    RectTransform rectTransform = null!;
    Transform originalParent = null!;
    public TextMeshProUGUI itemName = null!;
    public TextMeshProUGUI itemQuantity = null!;
    int originalSiblingIndex;
    Item? item;
    public InventorySource? source;
    public GameObject? copy;
    private GameObject placeholder = null!;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        placeholder = new GameObject("Placeholder");
        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(originalSiblingIndex);
        RectTransform placeholderRect = placeholder.AddComponent<RectTransform>();
        placeholderRect.sizeDelta = rectTransform.sizeDelta;

        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (item == null) return;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        }
        if (placeholder != null)
        {
            Destroy(placeholder);
        }
    }

#nullable enable
    public void SetItem(Item? item, InventorySource? sourceInventory = null)
#nullable disable
    {
        this.item = item;
        source = sourceInventory;
        itemName.text = item?.itemName ?? "";
        itemQuantity.text = item?.quantity.ToString() ?? "";
    }

    public Item GetItem() => item;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryGridItemUIElement draggedItem))
        {
            int targetIndex = transform.GetSiblingIndex();
            draggedItem.transform.SetParent(transform.parent);
            draggedItem.transform.SetSiblingIndex(targetIndex);
        }
        else if (eventData.pointerDrag.TryGetComponent(out EquipmentSlotUIElement draggedSlot))
        {
            if (draggedSlot.slotItem == null) return;
            PlayerCharacter character = CharacterInfoPage.instance.character;
            Item removedItem = draggedSlot.slotItem.Clone();
            character.equipmentSlots.equipmentSlots[draggedSlot.equipmentSlotEnum].item = null;
            draggedSlot.UpdateSlotItem(null);

            if (CharacterInfoPage.instance.currentSource == InventorySource.Player)
            {
                CharacterInfoPage.instance.character.AddToInventory(removedItem);
            }
            else if (CharacterInfoPage.instance.currentSource == InventorySource.Guild)
            {
                removedItem.AddToInventory(GuildManager.instance.stockpile);
            }

            InventoryGridItemUIElement newItem = Instantiate(CharacterInfoPage.instance.inventoryGridItemPrefab, transform.parent)
                .GetComponent<InventoryGridItemUIElement>();
            newItem.SetItem(removedItem, CharacterInfoPage.instance.currentSource);
            newItem.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

    }
}

public enum InventorySource
{
    Player,
    Guild
}
