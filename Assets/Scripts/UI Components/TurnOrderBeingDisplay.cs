using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TurnOrderBeingDisplay : MonoBehaviour
{
    private Being being;
    private TextMeshProUGUI characterName;
    private List<TextMeshProUGUI> textComponents;
    private HealthBar healthBar;
    private GameObject turnIndicator;


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


    public void UpdateBeing(Being b)
    {
        being = b;
    }
}
