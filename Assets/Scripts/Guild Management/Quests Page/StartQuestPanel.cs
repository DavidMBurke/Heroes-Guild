using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
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
        foreach (PlayerCharacter character in GuildManager.instance.unassignedEmployees) {
            if (character == null || characters.Contains(character)) 
                continue;
            CharacterListItem characterListItem = Instantiate(characterListItemPrefab, characterSelectionPanel.transform).GetComponent<CharacterListItem>();
            
            Button button = characterListItem.GetComponent<Button>();
            button.onClick.AddListener(() => SelectCharacter(index, character));
            characterListItem.SetCharacter(character);
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
            FormatAttributes(selectedCharacter.attributes.attributes) +

            $"Affinities:\n" +
            FormatAffinities(selectedCharacter.affinities.affinities) +

            $"Combat Skills:\n" + FormatSkills(selectedCharacter.combatSkills.skills);
        column2.text =
            $"Non-Combat Skills:\n" + FormatSkills(selectedCharacter.nonCombatSkills.skills);
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
}
