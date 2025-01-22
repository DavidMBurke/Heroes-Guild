using System.Collections.Generic;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public static GuildManager instance;
    public int coin; //base currency in copper, to be displayed in broken down denominations (100 copper -> 1 silver, 100 silver -> 1 gold)
    public List<PlayerCharacter> staff;
    public List<PlayerCharacter> charactersForHire;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public void RemoveCoin(int amount)
    {
        coin -= amount;
    }

    public void GenerateCharactersForHire(int amount)
    {
        charactersForHire.Clear();
        for (int i = 0; i < amount; i++)
        {
            charactersForHire.Add(PlayerCharacter.CreateNewCharacter());
        }
    }
}
