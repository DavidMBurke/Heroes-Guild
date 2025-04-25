using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages party members, their movement states, and group actions.
/// </summary>
public class PartyManager : MonoBehaviour
{
    public static PartyManager instance = null!;

    // ========== Party Setup ==========
    public GameObject playerCharacterPrefab = null!;
    public PlayerCharacter[] partyMembers = new PlayerCharacter[6];

    // ========== Movement Tracking ==========
    public List<PlayerCharacter> movementGroup = new();
    public Dictionary<PlayerCharacter, Coroutine> followerMovementCoroutines = new();

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Starts a coroutine to move a follower. Stops any existing movement first.
    /// </summary>
    /// <param name="follower">The follower to move.</param>
    /// <param name="routine">The movement coroutine.</param>
    public void StartFollowerMovement(PlayerCharacter follower, IEnumerator routine)
    {
        if (followerMovementCoroutines.TryGetValue(follower, out Coroutine existing))
        {
            StopCoroutine(existing);
        }

        Coroutine newCoroutine = StartCoroutine(routine);
        followerMovementCoroutines[follower] = newCoroutine;
    }

    /// <summary>
    /// Stops an active movement coroutine for a follower.
    /// </summary>
    /// <param name="follower">The follower whose movement should be stopped.</param>
    public void StopFollowerMovement(PlayerCharacter follower)
    {
        if (followerMovementCoroutines.TryGetValue(follower, out Coroutine existing))
        {
            StopCoroutine(existing);
            followerMovementCoroutines.Remove(follower);
        }
    }

    // ========== Movement Group Management ==========

    /// <summary>
    /// Adds a character to the movement group if not present, or removes them if already in it.
    /// </summary>
    public void AddOrRemoveCharacterFromMovementGroup(PlayerCharacter character)
    {
        if (movementGroup.Contains(character))
        {
            movementGroup.Remove(character);
        }
        else
        {
            movementGroup.Add(character);
        }
    }

    /// <summary>
    /// Adds a character to the movement group.
    /// </summary>
    public void AddCharacterToMovementGroup(PlayerCharacter character)
    {
        movementGroup.Add(character);
    }

    /// <summary>
    /// Removes a character from the movement group.
    /// </summary>
    public void RemoveCharacterFromMovementGroup(PlayerCharacter character)
    {
        movementGroup.Remove(character);
    }

    /// <summary>
    /// Clears all characters from the movement group.
    /// </summary>
    public void ClearCharactersFromMovementGroup()
    {
        movementGroup.Clear();
    }
}
