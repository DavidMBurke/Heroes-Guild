using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Canvas canvas;

    public void ShowText(string text, Vector3 worldPosition, Color color)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject instance = Instantiate(floatingTextPrefab, canvas.transform);
        instance.transform.position = screenPosition;

        FloatingText floatingText = instance.GetComponent<FloatingText>();
        floatingText.SetText(text, color);
    }
}
