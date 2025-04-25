using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the initialization and spawning of the game level, party members, and environment.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null!;

    // ========== Managers ==========
    public MapGenerator mapGenerator = null!;
    private PartyManager partyManager = null!;

    // ========== World Entities ==========
    public List<Enemy> enemies = null!;

    // ========== Configuration ==========
    [Range(0, 6)]
    public int partyCount;

    // ========== Unity Lifecycle ==========

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

    /// <summary>
    /// Sets up references to required managers and verifies their presence.
    /// </summary>
    private void InitializeManagers()
    {
        partyManager = PartyManager.instance;
        if (partyManager == null)
        {
            Debug.LogError("PartyManager instance not found.");
        }
    }

    /// <summary>
    /// Spawns player characters into the scene using the partyManager prefab.
    /// Rolls stats and sets them as scene-active.
    /// </summary>
    private void SpawnPartyMembers()
    {
        for (int i = 0; i < partyCount; i++)
        {
            GameObject newPlayer = Instantiate(partyManager.playerCharacterPrefab);
            if (newPlayer == null)
            {
                Debug.LogError($"Failed to instantiate playerCharacterPrefab at index {i}.");
                continue;
            }

            PlayerCharacter playerCharacter = newPlayer.GetComponent<PlayerCharacter>();
            if (playerCharacter == null)
            {
                Debug.LogError($"Prefab at index {i} does not contain a PlayerCharacter component.");
                continue;
            }

            playerCharacter.RollNewStats();
            playerCharacter.isInScene = true;
            partyManager.partyMembers[i] = playerCharacter;
        }

        if (partyManager.partyMembers[0] == null)
        {
            Debug.LogError("SpawnPartyMembers failed to create characters.");
        }

        // Set initial player selection
        ActionManager.instance.SelectCharacter(partyManager.partyMembers[0]);
    }

    /// <summary>
    /// Generates the level map using the assigned MapGenerator component.
    /// </summary>
    private void GenerateLevel()
    {
        if (mapGenerator == null)
        {
            Debug.LogError("MapGenerator is not assigned.");
            return;
        }

        mapGenerator.generateRandomForestMap();
    }
}
