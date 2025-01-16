using System.Collections;
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
    private PlayerCharacter character;

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
            character = PlayerCharacter.CreateNewCharacter();
        }
        column1.text =
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

            $"Combat Skills:\n" +
            $"Dodge: {character.combatSkills.dodge}\n" +
            $"Block: {character.combatSkills.block}\n" +
            $"Stealth: {character.combatSkills.stealth}\n" +
            $"Melee: {character.combatSkills.melee}\n" +
            $"Ranged: {character.combatSkills.ranged}\n" +
            $"Healing: {character.combatSkills.healing}\n" +
            $"Auras: {character.combatSkills.auras}\n" +
            $"Attack Spells: {character.combatSkills.attackSpells}\n" +
            $"Area Spells: {character.combatSkills.areaSpells}\n";


        column2.text =
            $"Non-Combat Skills:\n" +
            $"Cooking: {character.nonCombatSkills.cooking}\n" +
            $"Sentry: {character.nonCombatSkills.sentry}\n" +
            $"Fletching: {character.nonCombatSkills.fletching}\n" +
            $"Trapping: {character.nonCombatSkills.trapping}\n" +
            $"Herbalism: {character.nonCombatSkills.herbalism}\n" +
            $"Medicine: {character.nonCombatSkills.medicine}\n" +
            $"Leather Working: {character.nonCombatSkills.leatherWorking}\n" +
            $"Tailoring: {character.nonCombatSkills.tailoring}\n" +
            $"Alchemy: {character.nonCombatSkills.alchemy}\n" +
            $"Armor Smithing: {character.nonCombatSkills.armorSmithing}\n" +
            $"Weapon Smithing: {character.nonCombatSkills.weaponSmithing}\n" +
            $"Enchanting: {character.nonCombatSkills.enchanting}\n" +
            $"Mechanisms: {character.nonCombatSkills.mechanisms}\n" +
            $"Jewelry Crafting: {character.nonCombatSkills.jewelryCrafting}\n" +
            $"Mining: {character.nonCombatSkills.mining}\n" +
            $"Animal Handling: {character.nonCombatSkills.animalHandling}\n" +
            $"Cartography: {character.nonCombatSkills.cartography}\n" +
            $"Barter: {character.nonCombatSkills.barter}";
    }
}
