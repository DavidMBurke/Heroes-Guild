using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceRoll
{

    public int diceNumber;
    public int diceSize;
    public int mod;
    public int minRoll;
    public int maxRoll;
    public int result = -1;

    public DiceRoll(int diceNumber, int diceSize, int mod)
    {
        this.diceNumber = diceNumber;
        this.diceSize = diceSize;
        this.mod = mod;
        minRoll = diceNumber + mod;
        maxRoll = diceNumber * diceSize + mod;
        Roll();
    }

    /// <summary>
    /// Standard roll that attacks, saves, etc based on. Allows quick changing of base mechanics.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    public static DiceRoll StandardRoll(int mod)
    {
        return new(2, 100, mod);
    }

    public void Roll()
    {
        List<int> rolls = new(); // so later I can do reroll mechanics
        for (int m = 0; m < diceNumber; m++)
        {
            int roll = Random.Range(1, diceSize + 1);
            rolls.Add(roll);
        }
        result =  rolls.Sum() + mod;
    }

    public int GetResult()
    {
        if (result == -1)
        {
            Debug.LogError("Dice force rolled");
            Roll();
        }
        return result;
    }

    public string GetResultText()
    {
        return $"{diceNumber}d{diceSize}+{mod}";
    }
    
}
