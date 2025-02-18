using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character panels on the For Hire screen
/// </summary>
public class CharacterForHirePanel : MonoBehaviour
{
    private List<TextMeshProUGUI> tmps;
    private TextMeshProUGUI column1;
    private TextMeshProUGUI column2;
    public PlayerCharacter character;
    public Button characterMenuButton;
    public CharacterInfoPanel characterInfoPanel;


    void Start()
    {
        tmps = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        column1 = tmps.First(t => t.name == "Column 1 Text");
        column2 = tmps.First(t => t.name == "Column 2 Text");
    }

    void Update()
    {
        if (character == null)
        {
            column1.text = string.Empty;
            column2.text = string.Empty;
            characterMenuButton.gameObject.SetActive(false);
            characterMenuButton.onClick.RemoveAllListeners();
            characterMenuButton.onClick.AddListener(() => CharacterMenuButtonOnClickHandler());
            return;
        }
        characterMenuButton.gameObject.SetActive(true);
        column1.text =
            $"{character.characterName} \n" +
            $"Race: {character.race.name} \n" +
            $"Class: {character.playerClass.name} \n\n" +
            $"Attributes: \n" +
            FormatAttributes(character.attributes.attributes) +

            $"Affinities:\n" +
            FormatAffinities(character.affinities.affinities) +

            $"Combat Skills:\n" + FormatSkills(character.combatSkills.skills);


        column2.text =
            $"Non-Combat Skills:\n" + FormatSkills(character.nonCombatSkills.skills);
    }

    private void CharacterMenuButtonOnClickHandler()
    {
        characterInfoPanel.Open();
        characterInfoPanel.SetCharacter(character);
    }

    private string FormatAttributes(Dictionary<string, Attribute> attributeDict)
    {
        return string.Join("\n", attributeDict.Select(a => $"{a.Key}: {a.Value.level}")) + "\n\n";
    }
    private string FormatAffinities(Dictionary<string, Affinity> affinityDict)
    {
        return string.Join("\n", affinityDict.Select(a => $"{a.Key}: {a.Value.level}")) + "\n\n";
    }

    private string FormatSkills(Dictionary<string, Skill> skillDict)
    {
        return string.Join("\n", skillDict.Select(s => $"{s.Key}: {s.Value.level}")) + "\n\n";
    }

    public void AssignCharacter(PlayerCharacter playerCharacter)
    {
        character = playerCharacter;

    }
}
