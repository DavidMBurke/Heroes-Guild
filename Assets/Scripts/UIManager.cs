using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static bool CheckForUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

}
