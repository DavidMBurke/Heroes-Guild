using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName = null!;
    public string location = null!;
    public string description = null!;
    public int level;
    public int xpReward;
    public int coinReward;
    List<object> items = null!;
}
