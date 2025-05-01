using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string tooltipText;
    private InfoPopup popup;

    void Start()
    {
        popup = FindObjectOfType<InfoPopup>();
        if (popup == null)
        {
            Debug.LogError("InfoPopup not found in scene");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (popup == null)
        {
            return;
        }

        Vector3 screenPosition = eventData.position;
        popup.Display(screenPosition, tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popup?.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        popup?.Hide();
    }
}


