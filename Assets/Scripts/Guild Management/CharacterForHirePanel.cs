using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Character panels on the For Hire screen
/// </summary>
public class CharacterForHirePanel : MonoBehaviour
{
    private List<TextMeshProUGUI> tmps;
    private TextMeshProUGUI column1;
    private TextMeshProUGUI column2;
    public PlayerCharacter character;

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
            return;
        }
        column1.text =
            $"{character.characterName} \n" +
            $"Race: {character.race.name} \n" +
            $"Class: {character.playerClass.name} \n\n" +
            $"Attributes: \n" +
            $"Strength: {character.attributes.strength}\n" +
            $"Agility: {character.attributes.agility}\n" +
            $"Charisma: {character.attributes.charisma}\n" +
            $"Intelligence: {character.attributes.intelligence}\n" +
            $"Will: {character.attributes.will}\n" +
            $"Fortitude: {character.attributes.fortitude}\n\n" +

            $"Affinities:\n" +
            $"Nature: {character.affinities.nature}\n" +
            $"Arcana: {character.affinities.arcana}\n" +
            $"Celestial: {character.affinities.celestial}\n" +
            $"Spiritual: {character.affinities.spiritual}\n" +
            $"Mundane: {character.affinities.qi}\n\n" +

            $"Combat Skills:\n" + FormatSkills(character.combatSkills.skills);


        column2.text =
            $"Non-Combat Skills:\n" + FormatSkills(character.nonCombatSkills.skills);
    }

    private string FormatSkills(Dictionary<string, Skill> skillDict)
    {
        return string.Join("\n", skillDict.Select(s => $"{s.Key}: {s.Value.level}"));
    }

    public void AssignCharacter(PlayerCharacter playerCharacter)
    {
        character = playerCharacter;
    }
}
