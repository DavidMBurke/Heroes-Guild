using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    public PlayerCharacter player;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerLevel;
    public TextMeshProUGUI playerRace;
    public TextMeshProUGUI playerClass;

    private void Start()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        playerName = textItems.First(tmp => tmp.gameObject.name == "Name");
        playerLevel = textItems.First(tmp => tmp.gameObject.name == "Level");
        playerRace = textItems.First(tmp => tmp.gameObject.name == "Race");
        playerClass = textItems.First(tmp => tmp.gameObject.name == "Class");
    }

    public void UpdateSlot()
    {
        if (player == null)
        {
            playerName.text = "Empty slot";
            playerLevel.text = string.Empty;
            playerRace.text = string.Empty;
            playerClass.text = string.Empty;
            return;
        }
        playerName.text = player.characterName;
        playerLevel.text = player.level.ToString();
        playerRace.text = player.race.name;
        playerClass.text = player.playerClass.name;
    }
}
