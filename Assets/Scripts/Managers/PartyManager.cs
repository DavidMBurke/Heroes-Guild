using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Track Party Members and their states
/// </summary>
/// Note: Will be populated eventually with inter-party interaction states and logic
public class PartyManager : MonoBehaviour
{
    public GameObject playerCharacterPrefab = null!;
    public static PartyManager instance = null!;
    public PlayerCharacter[] partyMembers = new PlayerCharacter[6];
    public List<PlayerCharacter> movementGroup = new List<PlayerCharacter>();

    public Dictionary<PlayerCharacter, Coroutine> followerMovementCoroutines = new();

    public void StartFollowerMovement(PlayerCharacter follower, IEnumerator routine)
    {
        if (followerMovementCoroutines.TryGetValue(follower, out Coroutine existing))
        {
            if (existing != null)
            {
                StopCoroutine(existing);
            }
        }

        Coroutine newCoroutine = StartCoroutine(routine);
        followerMovementCoroutines[follower] = newCoroutine;
    }

    public void StopFollowerMovement(PlayerCharacter follower)
    {
        if (followerMovementCoroutines.TryGetValue(follower, out Coroutine existing))
        {
            StopCoroutine(existing);
            followerMovementCoroutines.Remove(follower);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void AddOrRemoveCharacterFromMovementGroup(PlayerCharacter character)
    {
        if (movementGroup.Contains(character))
        {
            movementGroup.Remove(character);
        } else
        {
            movementGroup.Add(character);
        }
    }

    public void AddCharacterToMovementGroup(PlayerCharacter character)
    {
        movementGroup.Add(character);
    }

    public void RemoveCharacterFromMovementGroup(PlayerCharacter character)
    {
        movementGroup.Remove(character);
    }

    public void ClearCharactersFromMovementGroup()
    {
        movementGroup.Clear();
    }

}
