using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    Being[] beings;
    public Button nextTurnButton;
    int beingsTurnIndex = 0;

    void Start()
    {
        beings = FindObjectsOfType<Being>();
        beings[beingsTurnIndex].isTurn = true;

        nextTurnButton.onClick.AddListener(NextTurn);

    }

    public void NextTurn()
    {
        foreach (Being being in beings)
        {
            being.isTurn = false;
        }
        beings[beingsTurnIndex].isTurn = false;
        beingsTurnIndex++;
        if (beingsTurnIndex >= beings.Length)
        {
            beingsTurnIndex = 0;
        }
        beings[beingsTurnIndex].isTurn = true;
    }

    
}
