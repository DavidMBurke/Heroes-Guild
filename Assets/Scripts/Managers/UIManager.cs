using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// State and UI-Specific Functions
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Return true if pointer is over a UI element
    /// </summary>
    /// <returns></returns>
    public static bool CheckForUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

}
