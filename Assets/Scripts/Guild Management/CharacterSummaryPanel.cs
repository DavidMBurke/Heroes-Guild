using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character panels on the For Hire screen
/// </summary>
public class CharacterSummaryPanel : MonoBehaviour
{
    public TextMeshProUGUI column1 = null!;
    public TextMeshProUGUI column2 = null!;
    public PlayerCharacter character = null!;
    public Button characterMenuButton = null!;
    public CharacterInfoPage characterInfoPage = null!;

    void UpdateText()
    {
        if (character == null)
        {
            column1.text = string.Empty;
            column2.text = string.Empty;
            characterMenuButton.gameObject.SetActive(false);
            return;
        }
        if (characterInfoPage != null)
        {
            characterMenuButton.gameObject.SetActive(true);
            characterMenuButton.onClick.RemoveAllListeners();
            characterMenuButton.onClick.AddListener(() => CharacterMenuButtonOnClickHandler());
        }
        column1.text =
            $"{character.characterName} \n" +
            $"Race: {character.race.name} \n" +
            $"Class: {character.playerClass.name} \n\n" +
            
            FormatAttributes(character.attributes.attributes) +
            FormatAffinities(character.affinities.affinities) +
            FormatCombatSkills(character.combatSkills.skills);


        column2.text =
            FormatNonCombatSkills(character.nonCombatSkills.skills);
    }

    private void CharacterMenuButtonOnClickHandler()
    {
        characterInfoPage.Open();
        characterInfoPage.SetCharacter(character);
    }

    private string FormatAttributes(Dictionary<string, Attribute> attributeDict)
    {
        return $"Attributes: \n" + string.Join("\n", attributeDict.Select(a => $"{a.Key}: {a.Value.level}")) + "\n\n";
    }

    private string FormatAffinities(Dictionary<string, Affinity> affinityDict)
    {
        return $"Affinities: \n" + string.Join("\n", affinityDict.Select(a => $"{a.Key}: {a.Value.level}")) + "\n\n";
    }

    private string FormatCombatSkills(Dictionary<string, Skill> skillDict)
    {
        return $"Combat Skills: \n" +
            string.Join("\n", skillDict.Select(s =>
                $"{s.Key}: {s.Value.modifiedLevel} {(s.Value.level == s.Value.modifiedLevel ? "" : $"(base: {s.Value.level})")}"
            )) + "\n\n";
    }

    private string FormatNonCombatSkills(Dictionary<string, Skill> skillDict)
    {
        return $"Non-Combat Skills: \n" +
            string.Join("\n", skillDict.Select(s =>
                $"{s.Key}: {s.Value.modifiedLevel} {(s.Value.level == s.Value.modifiedLevel ? "" : $"(base: {s.Value.level})")}"
            )) + "\n\n";
    }

    public void AssignCharacter(PlayerCharacter playerCharacter)
    {
        character = playerCharacter;
        UpdateText();
    }
}
