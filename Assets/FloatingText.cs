using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 20f;
    public float duration = 1f;
    public Vector3 screenOffset = new Vector3(30, 0, 0);

    private void Start()
    {
        Destroy(gameObject, duration);
        transform.position += screenOffset;
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void SetText(string message, Color color)
    {
        TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = message;
        tmp.color = color;
    }

}
