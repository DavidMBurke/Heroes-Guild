using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassThroughScrollRect : ScrollRect
{
    public override void OnDrag(PointerEventData eventData)
    {
        // Allow dragging (scrolling) only if not clicking on a button
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        // Make sure Unity registers drag events
        base.OnInitializePotentialDrag(eventData);
    }
}