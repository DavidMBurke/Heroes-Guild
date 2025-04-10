using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public PlayerCharacter player = null!;
    public TextMeshProUGUI playerName = null!;
    public TextMeshProUGUI playerLevel = null!;
    public TextMeshProUGUI playerRace = null!;
    public TextMeshProUGUI playerClass = null!;
    public Button removePlayerButton = null!;

    public void UpdateSlot()
    {
        if (player == null)
        {
            removePlayerButton.gameObject.SetActive(false);
            playerName.text = "Empty slot";
            playerLevel.text = string.Empty;
            playerRace.text = string.Empty;
            playerClass.text = string.Empty;
            return;
        }
        removePlayerButton.gameObject.SetActive(true);
        playerName.text = player.characterName;
        playerLevel.text = player.level.ToString();
        playerRace.text = player.race.name;
        playerClass.text = player.playerClass.name;
    }

    public void SetPlayersRemovable(bool removable)
    {
        removePlayerButton.gameObject.SetActive(removable);
    }
}
