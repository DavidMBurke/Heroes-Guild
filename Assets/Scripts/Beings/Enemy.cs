using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy NPC
/// </summary>
public class Enemy : Being
{
    public float enemyTurnTime = 3;

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Skip turn if not alive, set turn to true and initiate actions (currently a 3 second pause placeholder)
    /// </summary>
    /// <param name="combatManager"></param>
    public void StartTurn(ActionManager combatManager)
    {
        if (!isAlive)
        {
            combatManager.NextTurn();
            return;
        }
        isTurn = true;
        startingPosition = transform.position;
        StartCoroutine(EnemyTurnCoroutine(combatManager));
    }

    public void EndTurn()
    {
        isTurn = false;
    }

    private IEnumerator EnemyTurnCoroutine(ActionManager combatManager)
    {
        float timer = 0f;
        while (timer < enemyTurnTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isTurn = false;
        combatManager.NextTurn();
    }
}
