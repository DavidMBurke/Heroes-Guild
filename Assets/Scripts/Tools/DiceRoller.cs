using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceRoller
{

    /// <summary>
    /// Roll i dice with j sides and add mod.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="mod"></param>
    /// <returns></returns>
    public static int Roll(int i, int j, int mod = 0)
    {
        List<int> rolls = new(); // so later I can do reroll mechanics
        for (int m = 0; m < i; m++)
        {
            int roll = Random.Range(1, j + 1);
            rolls.Add(roll);
        }
        return rolls.Sum() + mod;
    }
}
