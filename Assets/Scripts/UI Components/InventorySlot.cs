using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Interactable interactable;
    public Image image;
    private Being being;

    /// <summary>
    /// Identify Image component
    /// </summary>
    private void Start()
    {
        image = GetComponentsInChildren<Image>().ToList().FirstOrDefault(i => i.name == "Icon");
    }

    /// <summary>
    /// Check for right click
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && IsMouseOverUI())
        {
            OnRightClick();
        }
    }

    /// <summary>
    /// Update Inventory Slot to given Interactable
    /// </summary>
    /// <param name="interactable"></param>
    public void UpdateItem(Interactable interactable)
    {
        if (interactable != null && interactable.sprite != null)
        {
            this.interactable = interactable;
            image.sprite = interactable.sprite;
            image.color = Color.white;
            return;
        }        
        image.sprite = null;
        image.color = Color.clear;
    }

    /// <summary>
    /// Show interaction popup on inventory item
    /// </summary>
    public void OnRightClick()
    {
        if (interactable == null)
        {
            return;
        }
        being = GetComponentInParent<InventoryDisplay>().currentPlayer;
        ActionManager.instance.ExecuteCharacterAction(new CharacterAction((player, action) => Interaction.InteractWithInventoryItem(player, action, interactable), being));
    }

    /// <summary>
    /// Check if mouse is over this element
    /// </summary>
    /// <returns></returns>
    private bool IsMouseOverUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Any(r => r.gameObject == gameObject);
    }
}
