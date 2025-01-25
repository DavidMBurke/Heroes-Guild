using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartQuestPanel : MonoBehaviour
{
    public GameObject characterList;
    public GameObject characterSelectionScroll;
    public GameObject characterSelectionPanel;
    public GameObject characterInfoPanel;
    public GameObject characterListItemPrefab;
    public CharacterSlot[] characterSlots;
    public PlayerCharacter[] characters = new PlayerCharacter[6];
    public int selectedIndex;
    public PlayerCharacter selectedCharacter;

    private void Start()
    {
        characterSlots = characterList.GetComponentsInChildren<CharacterSlot>();
        for (int i = 0; i < characterSlots.Length; i++) 
        {
            int index = i;
            Button button = characterSlots[i].GetComponent<Button>();
            button.onClick.AddListener(() => CharacterClickHandler(index));
        }
        UpdateSlots();
        characterSelectionScroll.SetActive(false);
        characterInfoPanel.SetActive(false);
    }

    void CharacterClickHandler(int index)
    {
        Debug.Log(index);
        if (characters[index] == null)
        {
            DisplayCharacterSelection(index);
            return;
        }
        DisplayCharacterInfo(characters[index]);
    }

    public void DisplayCharacterInfo(PlayerCharacter character)
    {

        characterSelectionScroll.SetActive(false);
        characterInfoPanel.SetActive(true);
    }

    public void DisplayCharacterSelection(int index)
    {        
        characterInfoPanel.SetActive(false);
        characterSelectionScroll.SetActive(true);
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
    }

    public void SelectCharacter(int index, PlayerCharacter character)
    {
        selectedIndex = index;
        selectedCharacter = character;
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].player = characters[i];
            characterSlots[i].UpdateSlot();
        }
    }
}
