using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null!;
    public MapGenerator mapGenerator = null!;
    private PartyManager partyManager = null!;
    public List<Enemy> enemies = null!;
    [Range(0, 6)]
    public int partyCount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeManagers();
        SpawnPartyMembers();
        GenerateLevel();
    }

    private void InitializeManagers()
    {
        partyManager = PartyManager.instance;
        if (partyManager == null)
        {
            Debug.LogError("PartyManager instance not found");
        }
    }

    /// <summary>
    /// Placeholder logic, currently generates random characters, will load in characters in the future
    /// </summary>
    private void SpawnPartyMembers()
    {
        for (int i = 0; i < partyCount; i++)
        {
            GameObject newPlayer = Instantiate(partyManager.playerCharacterPrefab);
            if (newPlayer == null)
            {
                Debug.LogError($"Failed to instantiate playerCharacterPrefab at index {i}");
                continue;
            }

            PlayerCharacter playerCharacter = newPlayer.GetComponent<PlayerCharacter>();
            if (playerCharacter == null)
            {
                Debug.LogError($"Prefab at index {i} does not contain a PlayerCharacter component.");
                continue;
            }
            playerCharacter.RollNewStats();
            partyManager.partyMembers[i] = playerCharacter;
        }
        if (partyManager.partyMembers[0] == null)
        {
            Debug.LogError("SpawnPartyMembers failed to create characters");
        }
        ActionManager.instance.SelectCharacter(partyManager.partyMembers[0]);
    }

    private void GenerateLevel()
    {
        if (mapGenerator == null)
        {
            Debug.LogError("MapGenerator is not assigned");
            return;
        }
        mapGenerator.generateRandomForestMap();
    }


}
