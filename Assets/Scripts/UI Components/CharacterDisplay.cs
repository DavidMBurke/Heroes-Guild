using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterDisplay : MonoBehaviour
{
    public PartyManager partyManager;
    public int partyIndex;
    private PlayerCharacter character;
    private TextMeshProUGUI characterName;
    private TextMeshProUGUI health;
    private TextMeshProUGUI actionPoints;
    private List<TextMeshProUGUI> textComponents;
    private GameObject actionPointsObject;
    private GameObject healthBar;
    private ActionManager actionManager;
    
    private void Start()
    {
        actionManager = ActionManager.instance;
        textComponents = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        characterName = textComponents.FirstOrDefault(t => t.name == "Name Text");
        health = textComponents.FirstOrDefault(t => t.name == "Health Text");
        actionPoints = textComponents.FirstOrDefault(t => t.name == "Action Point Text");
        actionPointsObject = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Action Points")?.gameObject;
        healthBar = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Health")?.gameObject;
    }

    private void Update()
    {
        character = partyManager.partyMembers[partyIndex];
        if (character == null)
        {
            gameObject.SetActive(false);
            return;
        }
        bool showActionPoints = character.isTurn && actionManager.IsTurnBasedMode();
        actionPointsObject.gameObject.SetActive(showActionPoints);
        gameObject.SetActive(true);
        characterName.text = character.characterName;
        health.text = $"{character.health}/{character.maxHealth}";
        actionPoints.text = character.actionPoints.ToString();
        Vector3 healthBarScale = healthBar.gameObject.transform.localScale;
        healthBarScale.x = Mathf.Max((float)character.health / character.maxHealth, 0);

        healthBar.gameObject.transform.localScale = healthBarScale;
    }
    public void OnClick()
    {
        if (actionManager.IsTurnBasedMode())
        {
            return;
        }
        actionManager.SelectCharacter(character);
    }
}
