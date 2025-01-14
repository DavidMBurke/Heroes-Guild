using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Enemy NPC
/// </summary>
public class Enemy : Being
{
    public float enemyTurnTime = 3;
    public int group;
    public bool isAwareOfPlayers = false;
    public GameObject sightSphere;
    List<Being> overlappingBeings = new List<Being>();
    OverlapDetector detector;

    new void Start()
    {
        base.Start();
        detector = sightSphere.AddComponent<OverlapDetector>();
        detector.SetBeingList(overlappingBeings);
    }

    private new void Update()
    {
        base.Update();
        if (detector != null && !isAwareOfPlayers && overlappingBeings.OfType<PlayerCharacter>().ToList().Count > 0)
        {
            SpotPlayers();
        }
       

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

    public void SpotPlayers()
    {
        if (!isAlive)
        {
            return;
        }
        List<Enemy> groupEnemies = LevelManager.instance.enemies.Where(e => e.group == group).ToList();
        foreach (Enemy enemy in groupEnemies)
        {
            enemy.isAwareOfPlayers = true;
        }
        ActionManager.instance.currentBeing.currentAction.endSignal = true;
    }
}
