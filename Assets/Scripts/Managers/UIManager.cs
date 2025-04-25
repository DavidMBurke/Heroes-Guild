using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles UI state queries and UI-specific utility functions.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Checks if the mouse pointer is currently over a UI element.
    /// </summary>
    /// <returns>True if the pointer is over a UI element; otherwise, false.</returns>
    public static bool CheckForUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
