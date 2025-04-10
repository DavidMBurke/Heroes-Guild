using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestManagementPanel : MonoBehaviour
{
    public GameObject characterList = null!;
    public GameObject characterSelection = null!;
    public GameObject characterSelectionPanel = null!;
    public GameObject characterInfo = null!;
    public GameObject characterListItemPrefab = null!;
    public CharacterSlot[] characterSlots = null!;
    public PlayerCharacter[] characters = new PlayerCharacter[6];
    public PlayerCharacter selectedCharacter = null!;
    public int selectedIndex;
    private TextMeshProUGUI column1 = null!;
    private TextMeshProUGUI column2 = null!;
    public Quest currentQuest = null!;
    public TextMeshProUGUI questName = null!;
    public TextMeshProUGUI description = null!;
    public TextMeshProUGUI location = null!;
    public Button questProgressionButton = null!;
    private TextMeshProUGUI questProgressionButtonText = null!;
    public TextMeshProUGUI questProgressionText = null!;
    public QuestsPage questsPage = null!;
    public GameObject noCharactersAvailableText = null!;



    private void Update()
    {
        questProgressionButton.onClick.RemoveAllListeners();

        foreach (CharacterSlot slot in characterSlots)
        {
            slot.SetPlayersRemovable(currentQuest.questStatus == Quest.StatusEnum.NotStarted);
        }

        switch (currentQuest.questStatus)
        {
            case Quest.StatusEnum.NotStarted:
                questProgressionText.gameObject.SetActive(false);
                questProgressionButton.gameObject.SetActive(true);
                questProgressionButtonText.text = "Start Quest";
                questProgressionButton.onClick.AddListener(() => StartQuest());
                break;

            case Quest.StatusEnum.Travelling:
                questProgressionText.gameObject.SetActive(true);
                questProgressionButton.gameObject.SetActive(false);
                questProgressionText.text = $"Traveling To Destination\n{currentQuest.remainingTime / 60:D2}:{currentQuest.remainingTime % 60:D2} remaining";
                break;

            case Quest.StatusEnum.Ready:
                questProgressionText.gameObject.SetActive(false);
                questProgressionButton.gameObject.SetActive(true);
                questProgressionButtonText.text = "Enter Dungeon";
                questProgressionButton.onClick.AddListener(() => EnterDungeon());
                break;

            case Quest.StatusEnum.InDungeon:
                questProgressionText.gameObject.SetActive(true);
                questProgressionButton.gameObject.SetActive(false);
                questProgressionText.text = $"In Dungeon\n{currentQuest.remainingTime / 60:D2}:{currentQuest.remainingTime % 60:D2} remaining";
                break;

            case Quest.StatusEnum.DungeonComplete:
                questProgressionText.gameObject.SetActive(false);
                questProgressionButton.gameObject.SetActive(true);
                questProgressionButtonText.text = "Return To Guild";
                questProgressionButton.onClick.AddListener(() => ReturnToGuild());
                break;

            case Quest.StatusEnum.Returning:
                questProgressionText.gameObject.SetActive(true);
                questProgressionButton.gameObject.SetActive(false);
                questProgressionText.text = $"Returning to Guild\n{currentQuest.remainingTime / 60:D2}:{currentQuest.remainingTime % 60:D2} remaining";
                break;

            case Quest.StatusEnum.QuestComplete:
                questProgressionText.gameObject.SetActive(false);
                questProgressionButton.gameObject.SetActive(true);
                questProgressionButtonText.text = "Complete Quest";
                questProgressionButton.onClick.AddListener(() => CompleteQuest());
                break;

            default:
                Debug.LogError("Quest status in switch not accounted for");
                break;
        }
    }
    private void Start()
    {
        questProgressionButtonText = questProgressionButton.GetComponentInChildren<TextMeshProUGUI>();
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
    public void SetQuest(Quest quest)
    {
        currentQuest = quest;
        questName.text = quest.name;
        description.text = quest.description;
        location.text = quest.location;
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = i < quest.characters.Count ? quest.characters[i] : null!;
        }
        UpdateSlots();
    }

    void CharacterClickHandler(int index)
    {
        if (characters[index] == null)
        {
            if (currentQuest.questStatus == Quest.StatusEnum.NotStarted)
            {
                DisplayCharacterSelection(index);
            }
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
            button.onClick.AddListener(() => SelectCharacter(index, character, characterListItem));
            characterListItem.SetCharacter(character);
            if (character == selectedCharacter)
            {
                characterListItem.SetHighlight();
            }
        }
        noCharactersAvailableText.gameObject.SetActive(GuildManager.instance.unassignedEmployees.Count == 0);
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
        GuildManager.instance.assignedToQuest.Add(selectedCharacter);
        GuildManager.instance.unassignedEmployees.Remove(selectedCharacter);
        UpdateQuestCharacters();
        UpdateSlots();
        characterSelection.SetActive(false);
    }

    public void SelectCharacter(int index, PlayerCharacter character, CharacterListItem characterListItem)
    {
        selectedIndex = index;
        selectedCharacter = character;
        DisplayCharacterSelection(index);
    }

    public void RemoveCharacter(int index)
    {
        GuildManager.instance.unassignedEmployees.Add(selectedCharacter);
        GuildManager.instance.assignedToQuest.Remove(selectedCharacter);
        characters[index] = null;
        UpdateQuestCharacters();
        UpdateSlots();
        if (selectedIndex == index)
        {
            characterSelection.SetActive(false);
            characterInfo.SetActive(false);
        }
        if (characterSelection.gameObject.activeInHierarchy)
        {
            DisplayCharacterSelection(index);
        }
    }

    private void UpdateQuestCharacters()
    {
        currentQuest.characters = characters.Where(c => c != null).ToList();
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

    public void StartQuest()
    {
        if (currentQuest.characters.Count() < 1)
        {
            AlertManager.instance.ShowAlert("At least one adventurer required to begin quest");
            return;
        }
        currentQuest.StartQuest();
    }

    public void EnterDungeon()
    {
        currentQuest.EnterDungeon();
    }

    public void ReturnToGuild()
    {
        currentQuest.ReturnToGuild();
    }

    public void CompleteQuest()
    {
        currentQuest.CompleteQuest();
        GuildManager.instance.AddCoin(currentQuest.coinReward);
        questsPage.RemoveQuest(currentQuest);
        gameObject.SetActive(false);
        Destroy(currentQuest);
    }
}
