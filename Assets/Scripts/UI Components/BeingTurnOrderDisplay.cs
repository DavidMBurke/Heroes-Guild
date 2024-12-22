using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BeingTurnOrderDisplay : MonoBehaviour
{
    public CombatManager combatManager;
    public int beingIndex;
    private Being being;
    private TextMeshProUGUI characterName;
    private TextMeshProUGUI health;
    private List<TextMeshProUGUI> textComponents;
    private GameObject healthBar;
    private GameObject turnIndicator;


    private void Start()
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        characterName = textComponents.FirstOrDefault(t => t.name == "Name Text");
        health = textComponents.FirstOrDefault(t => t.name == "Health Text");
        healthBar = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Health")?.gameObject;
        turnIndicator = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Turn Indicator")?.gameObject;
    }
    private void Update()
    {
        if (combatManager.beings.Length <= beingIndex) // There should only be as many turn order displays as beings in combat
        {
            Destroy(gameObject);
            return;
        }
        being = combatManager.beings[beingIndex];
        turnIndicator.SetActive(being.isTurn);
        characterName.text = being.characterName;
        health.text = $"{being.health}/{being.maxHealth}";
        Vector3 healthBarScale = healthBar.gameObject.transform.localScale;
        healthBarScale.x = Mathf.Max((float)being.health / being.maxHealth, 0);
        healthBar.gameObject.transform.localScale = healthBarScale;
    }
}
