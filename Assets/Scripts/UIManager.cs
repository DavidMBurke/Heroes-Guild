using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static void CheckForUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }

}
