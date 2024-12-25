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

    private void Start()
    {
        image = GetComponentsInChildren<Image>().ToList().FirstOrDefault(i => i.name == "Icon");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && IsMouseOverUI())
        {
            OnRightClick();
        }
    }

    public void UpdateItem(Interactable i)
    {
        if (i != null && i.sprite != null)
        {
            interactable = i;
            image.sprite = i.sprite;
            image.color = Color.white;
            return;
        }        
        image.sprite = null;
        image.color = Color.clear;
    }

    public void OnRightClick()
    {
        if (interactable == null)
        {
            return;
        }
        being = GetComponentInParent<InventoryDisplay>().currentPlayer;
        ActionManager.instance.ExecuteCharacterAction(new CharacterAction((player, action) => Interaction.InteractWithInventoryItem(player, action, interactable), being));
    }

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
