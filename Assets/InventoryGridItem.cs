using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGridItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Transform originalParent;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
#nullable enable
    Item? item;
    public InventorySource? source;
    public GameObject? copy;
#nullable disable


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        originalParent = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(Item? item, InventorySource? sourceInventory = null)
    {
        this.item = item;
        source = sourceInventory;
        itemName.text = item == null ? "" : item.itemName;
        itemQuantity.text = item == null ? "" : item.quantity.ToString();
    }

    public Item GetItem() => item;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryGridItem draggedItem))
        {
            int targetIndex = transform.GetSiblingIndex();
            draggedItem.transform.SetParent(transform.parent);
            draggedItem.transform.SetSiblingIndex(targetIndex);
        }
    }
}

public enum InventorySource
{
    Player,
    Guild
}
