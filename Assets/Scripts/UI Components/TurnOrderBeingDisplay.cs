using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Component containing turn order icons
/// </summary>
public class TurnOrderBeingDisplay : MonoBehaviour
{
    private Being being = null!;
    private TextMeshProUGUI characterName = null!;
    private List<TextMeshProUGUI> textComponents = null!;
    private HealthBar healthBar = null!;
    private GameObject turnIndicator = null!;


    private void Start()
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        characterName = textComponents.FirstOrDefault(t => t.name == "Name Text");
        healthBar = GetComponentInChildren<HealthBar>();
        turnIndicator = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Turn Indicator")?.gameObject;
    }
    private void Update()
    {
        if (being == null) return;
        turnIndicator.SetActive(being.isTurn);
        characterName.text = being.characterName;
        healthBar.UpdateHealthBar(being.health, being.maxHealth);
    }

    /// <summary>
    /// Update display slot with selected being
    /// </summary>
    /// <param name="b"></param>
    public void UpdateBeing(Being b)
    {
        being = b;
    }
}
