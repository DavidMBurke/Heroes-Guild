using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartQuestPanel : MonoBehaviour
{
    public GameObject characterList;
    public GameObject characterSelection;
    public GameObject characterSelectionPanel;
    public GameObject characterInfo;
    public GameObject characterListItemPrefab;
    public CharacterSlot[] characterSlots;
    public PlayerCharacter[] characters = new PlayerCharacter[6];
    public int selectedIndex;
    public PlayerCharacter selectedCharacter;
    private TextMeshProUGUI column1;
    private TextMeshProUGUI column2;

    private void Start()
    {
        List<TextMeshProUGUI> tmps = characterInfo.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        column1 = tmps.First(t => t.name == "Column 1 Text");
        column2 = tmps.First(t => t.name == "Column 2 Text");
        characterSlots = characterList.GetComponentsInChildren<CharacterSlot>();
        for (int i = 0; i < characterSlots.Length; i++) 
        {
            int index = i;
            Button button = characterSlots[i].GetComponent<Button>();
            button.onClick.AddListener(() => CharacterClickHandler(index));
            characterSlots[i].removePlayerButton.onClick.AddListener(() => RemoveCharacter(index));
        }
        UpdateSlots();
        characterSelection.SetActive(false);
        characterInfo.SetActive(false);
    }

    void CharacterClickHandler(int index)
    {
        if (characters[index] == null)
        {
            DisplayCharacterSelection(index);
            return;
        }
        DisplayCharacterInfo(characters[index]);
    }

    public void DisplayCharacterInfo(PlayerCharacter character)
    {

        characterSelection.SetActive(false);
        characterInfo.SetActive(true);
        UpdateCharacterInfoPanel();
    }

    public void DisplayCharacterSelection(int index)
    {        
        characterInfo.SetActive(false);
        characterSelection.SetActive(true);
        ClearCharacterSelection();
        foreach (PlayerCharacter character in GuildManager.instance.employees) {
            if (character == null || characters.Contains(character)) 
                continue;
            CharacterListItem characterListItem = Instantiate(characterListItemPrefab, characterSelectionPanel.transform).GetComponent<CharacterListItem>();
            Button button = characterListItem.GetComponent<Button>();
            button.onClick.AddListener(() => SelectCharacter(index, character));
            characterListItem.player = character;
        }
    }

    public void ClearCharacterSelection()
    {
        foreach (Transform child in characterSelectionPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AssignCharacter()
    {
        characters[selectedIndex] = selectedCharacter;
        UpdateSlots();
        characterSelection.SetActive(false);
    }

    public void SelectCharacter(int index, PlayerCharacter character)
    {
        selectedIndex = index;
        selectedCharacter = character;
    }

    public void RemoveCharacter(int index)
    {
        characters[index] = null;
        UpdateSlots();
        if (selectedIndex == index)
        {
            characterSelection.SetActive(false);
            characterInfo.SetActive(false);
        }
    }


    public void UpdateSlots()
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].player = characters[i];
            characterSlots[i].UpdateSlot();
        }
    }

    public void UpdateCharacterInfoPanel()
    {
        if (selectedCharacter == null)
        {
            column1.text = string.Empty;
            column2.text = string.Empty;
            return;
        }
        column1.text =
            $"{selectedCharacter.characterName} \n" +
            $"Race: {selectedCharacter.race.name} \n" +
            $"Class: {selectedCharacter.playerClass.name} \n\n" +
        $"Attributes: \n" +
            $"Strength: {selectedCharacter.attributes.strength}\n" +
            $"Agility: {selectedCharacter.attributes.agility}\n" +
            $"Charisma: {selectedCharacter.attributes.charisma}\n" +
            $"Intelligence: {selectedCharacter.attributes.intelligence}\n" +
            $"Will: {selectedCharacter.attributes.will}\n" +
            $"Fortitude: {selectedCharacter.attributes.fortitude}\n\n" +
        $"Affinities:\n" +
            $"Nature: {selectedCharacter.affinities.nature}\n" +
            $"Arcana: {selectedCharacter.affinities.arcana}\n" +
            $"Celestial: {selectedCharacter.affinities.celestial}\n" +
            $"Spiritual: {selectedCharacter.affinities.spiritual}\n" +
            $"Mundane: {selectedCharacter.affinities.qi}\n\n" +
        $"Combat Skills:\n" +
            $"Dodge: {selectedCharacter.combatSkills.dodge}\n" +
            $"Block: {selectedCharacter.combatSkills.block}\n" +
            $"Stealth: {selectedCharacter.combatSkills.stealth}\n" +
            $"Melee: {selectedCharacter.combatSkills.melee}\n" +
            $"Ranged: {selectedCharacter.combatSkills.ranged}\n" +
            $"Healing: {selectedCharacter.combatSkills.healing}\n" +
            $"Auras: {selectedCharacter.combatSkills.auras}\n" +
            $"Evocation: {selectedCharacter.combatSkills.evocation}\n";


        column2.text =
            $"Non-Combat Skills:\n" +
            $"Cooking: {selectedCharacter.nonCombatSkills.cooking}\n" +
            $"Sentry: {selectedCharacter.nonCombatSkills.sentry}\n" +
            $"Fletching: {selectedCharacter.nonCombatSkills.fletching}\n" +
            $"Trapping: {selectedCharacter.nonCombatSkills.trapping}\n" +
            $"Herbalism: {selectedCharacter.nonCombatSkills.herbalism}\n" +
            $"Medicine: {selectedCharacter.nonCombatSkills.medicine}\n" +
            $"Leather Working: {selectedCharacter.nonCombatSkills.leatherWorking}\n" +
            $"Tailoring: {selectedCharacter.nonCombatSkills.tailoring}\n" +
            $"Alchemy: {selectedCharacter.nonCombatSkills.alchemy}\n" +
            $"Armor Smithing: {selectedCharacter.nonCombatSkills.armorSmithing}\n" +
            $"Weapon Smithing: {selectedCharacter.nonCombatSkills.weaponSmithing}\n" +
            $"Enchanting: {selectedCharacter.nonCombatSkills.enchanting}\n" +
            $"Mechanisms: {selectedCharacter.nonCombatSkills.mechanisms}\n" +
            $"Jewelry Crafting: {selectedCharacter.nonCombatSkills.jewelryCrafting}\n" +
            $"Mining: {selectedCharacter.nonCombatSkills.mining}\n" +
            $"Animal Handling: {selectedCharacter.nonCombatSkills.animalHandling}\n" +
            $"Cartography: {selectedCharacter.nonCombatSkills.cartography}\n" +
            $"Barter: {selectedCharacter.nonCombatSkills.barter}";
    }
}
