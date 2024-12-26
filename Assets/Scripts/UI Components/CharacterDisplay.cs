using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public PartyManager partyManager;
    public int partyIndex;
    private PlayerCharacter character;
    private TextMeshProUGUI characterName;
    private TextMeshProUGUI actionPoints;
    private List<TextMeshProUGUI> textComponents;
    private GameObject actionPointsObject;
    private HealthBar healthBar;
    private ActionManager actionManager;
    
    private void Start()
    {
        actionManager = ActionManager.instance;
        textComponents = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        healthBar = GetComponentInChildren<HealthBar>();
        characterName = textComponents.FirstOrDefault(t => t.name == "Name Text");
        actionPoints = textComponents.FirstOrDefault(t => t.name == "Action Point Text");
        actionPointsObject = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Action Points")?.gameObject;
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
        actionPoints.text = character.actionPoints.ToString();
        healthBar.UpdateHealthBar(character.health, character.maxHealth);
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
