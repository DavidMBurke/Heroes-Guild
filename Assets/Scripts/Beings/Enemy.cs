using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Being
{
    public float enemyTurnTime = 3;

    new void Start()
    {
        base.Start();
    }

    public void StartTurn(CombatManager combatManager)
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

    private IEnumerator EnemyTurnCoroutine(CombatManager combatManager)
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
