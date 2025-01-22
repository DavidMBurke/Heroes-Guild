using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Track Party Members and their states
/// </summary>
/// Note: Will be populated eventually with inter-party interaction states and logic
public class PartyManager : MonoBehaviour
{
    public GameObject playerCharacterPrefab;
    public static PartyManager instance;
    public PlayerCharacter[] partyMembers = new PlayerCharacter[6];
    public List<PlayerCharacter> movementGroup = new List<PlayerCharacter>();

    private void Awake()
    {
        instance = this;
    }

}
