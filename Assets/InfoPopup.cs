using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InfoPopup : MonoBehaviour
{
    public TextMeshProUGUI tmp = null!;
    public Vector3 offScreenPosition;
    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        offScreenPosition = new Vector3(-500, -700, 0);
        Hide();
    }

    /// <summary>
    /// Move the popup to the cursor position and include the applicable options
    /// </summary>
    /// <param name="text"></param>

    public void Display(Vector3? newPosition, string newText = "")
    {
        if (rect == null) return;

        Vector3 screenPos = newPosition ?? Input.mousePosition;

        rect.position = screenPos + new Vector3(10f, -10f, 0);

        if (!string.IsNullOrEmpty(newText))
            tmp.text = newText;
    }

    /// <summary>
    /// Hide the popup off screen
    /// </summary>
    public void Hide()
    {
        if (rect != null)
        {
            rect.anchoredPosition = offScreenPosition;
        }
    }

}
