using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterListItem : MonoBehaviour
{
    private PlayerCharacter player = null!;
    private TextMeshProUGUI displayName = null!;
    private TextMeshProUGUI level = null!;

    private void Awake()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        displayName = textItems.First(tmp => tmp.gameObject.name == "Name");
        level = textItems.First(tmp => tmp.gameObject.name == "Level");
    }

    public void SetCharacter(PlayerCharacter newPlayer)
    {
        player = newPlayer;
        UpdateDisplayInfo();
    }

    public void SetDisplayName(string name)
    {
        displayName.text = name;
    }

    private void UpdateDisplayInfo()
    {
        if (player == null)
        {
            Debug.LogError("No player assigned to CharacterListItem");
        }
        displayName.text = player.characterName;
        level.text = player.level.ToString();
    }

    public PlayerCharacter GetPlayer()
    {
        return player;
    }
}
