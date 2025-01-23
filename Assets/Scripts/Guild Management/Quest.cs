using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName;
    public string location;
    public string description;
    public int level;
    public int xpReward;
    public int coinReward;
    List<object> items;
}
