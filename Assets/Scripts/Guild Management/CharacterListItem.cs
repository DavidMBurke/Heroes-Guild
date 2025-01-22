using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterListItem : MonoBehaviour
{
    public PlayerCharacter player;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerLevel;

    private void Start()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        playerName = textItems.First(tmp => tmp.gameObject.name == "Name");
        playerLevel = textItems.First(tmp => tmp.gameObject.name == "Level");
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("No player assigned to CharacterListItem");
        }
        playerName.text = player.characterName;
        playerLevel.text = player.level.ToString();
    }
}
