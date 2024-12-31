using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public MapGenerator mapGenerator;
    private PartyManager partyManager;
    public List<Enemy> enemies;
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

            partyManager.partyMembers[i] = playerCharacter;
        }
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
