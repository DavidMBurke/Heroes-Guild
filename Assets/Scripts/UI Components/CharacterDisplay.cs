using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Display for individual characters in the party bar
/// </summary>
public class CharacterDisplay : MonoBehaviour
{
    public PartyManager partyManager = null!;
    public int partyIndex;
    private PlayerCharacter character = null!;
    private TextMeshProUGUI characterName = null!;
    private TextMeshProUGUI actionPoints = null!;
    private List<TextMeshProUGUI> textComponents = null!;
    private GameObject actionPointsObject = null!;
    private HealthBar healthBar = null!;
    private ActionManager actionManager = null!;
    public Outline outline;
    public Color movementGroupColor;
    public Color selectedColor;
    
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
        if (PartyManager.instance.movementGroup.Contains(character))
        {
            outline.enabled = true;
            outline.effectColor = movementGroupColor;
        } else if (character == actionManager.currentBeing)
        {
            outline.enabled = true;
            outline.effectColor = selectedColor;
        } else
        {
            outline.enabled = false;
        }
    }

    public void OnClick()
    {
        if (actionManager.IsTurnBasedMode())
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (character != actionManager.currentBeing)
            {
                partyManager.AddOrRemoveCharacterFromMovementGroup(character);
            }
        } 
        else
        {
            partyManager.ClearCharactersFromMovementGroup();
            actionManager.SelectCharacter(character);
        }
    }
}
