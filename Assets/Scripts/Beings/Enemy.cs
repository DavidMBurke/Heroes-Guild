using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Enemy NPC that can detect players and participate in turn-based combat.
/// </summary>
public class Enemy : Being
{
    // =====================
    // Enemy-Specific Fields
    // =====================
    public float enemyTurnTime = 3f;
    public int group;
    public bool isAwareOfPlayers = false;

    public GameObject sightSphere = null!;
    private List<Being> overlappingBeings = new List<Being>();
    private OverlapDetector detector = null!;

    /// <summary>
    /// Unity Start method. Initializes vision detector and base class.
    /// </summary>
    new void Start()
    {
        base.Start();
        detector = sightSphere.AddComponent<OverlapDetector>();
        detector.SetBeingList(overlappingBeings);
    }

    /// <summary>
    /// Unity Update method. Checks for player detection and base behavior.
    /// </summary>
    private new void Update()
    {
        base.Update();

        if (detector != null && !isAwareOfPlayers && overlappingBeings.OfType<PlayerCharacter>().Any())
        {
            SpotPlayers();
        }
    }

    /// <summary>
    /// Starts the enemy's turn. Skips if dead, otherwise begins a coroutine for action delay.
    /// </summary>
    /// <param name="combatManager">The action manager controlling turn order.</param>
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

    /// <summary>
    /// Ends the enemy's turn.
    /// </summary>
    public void EndTurn()
    {
        isTurn = false;
    }

    /// <summary>
    /// Coroutine that simulates a thinking delay during the enemy's turn.
    /// </summary>
    /// <param name="combatManager">The action manager controlling turn order.</param>
    /// <returns>Waits for the duration of enemyTurnTime before ending turn.</returns>
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

    /// <summary>
    /// Called when the enemy detects a player. Flags all group members as aware and ends the current player action.
    /// </summary>
    public void SpotPlayers()
    {
        if (!isAlive) return;

        List<Enemy> groupEnemies = LevelManager.instance.enemies
            .Where(e => e.group == group)
            .ToList();

        foreach (Enemy enemy in groupEnemies)
        {
            enemy.isAwareOfPlayers = true;
        }

        ActionManager.instance.currentBeing.currentAction.endSignal = true;
    }
}
