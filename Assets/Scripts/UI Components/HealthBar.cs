using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Health Bar component
/// </summary>
public class HealthBar : MonoBehaviour
{
    private List<Image> images = null!;
    private Image image = null!;
    private TextMeshProUGUI tmp = null!;

    private void Start()
    {
        images = GetComponentsInChildren<Image>().ToList();
        image = images.FirstOrDefault(i => i.name == "Health");
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Update text and scale health overlay
    /// </summary>
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float healthPercent = Mathf.Max((float)currentHealth / maxHealth, 0);
        image.color = SetHealthBarColor(healthPercent);
        SetHealthBarScale(healthPercent);
        tmp.text = $"{currentHealth}/{maxHealth}";
    }

    /// <summary>
    /// Scale health bar proportional to health
    /// </summary>
    /// <param name="healthPercent"></param>
    private void SetHealthBarScale(float healthPercent)
    {
        Vector3 healthBarScale = transform.localScale;
        healthBarScale.x = healthPercent;
        image.transform.localScale = healthBarScale;
    }

    /// <summary>
    /// Set health bar color to range from green -> yellow -> red
    /// </summary>
    /// <param name="healthPercent"></param>
    /// <returns></returns>
    private Color SetHealthBarColor(float healthPercent)
    {
        float redAmount = healthPercent > .5f ? 1 - ((healthPercent - .5f) / .5f) : 1;
        float greenAmount = healthPercent > .5f ? 1 : healthPercent * 2;
        return new Color(redAmount, greenAmount, 0);
    }
}
