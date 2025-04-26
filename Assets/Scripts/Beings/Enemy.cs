using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

    public float attackRange = 1.5f;
    public int attackDamage = 10;

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
    /// Starts the enemy's turn. Skips if dead, otherwise moves towards the closest player and attacks.
    /// </summary>
    public void StartTurn()
    {
        if (!isAlive)
        {
            ActionManager.instance.NextTurn();
            return;
        }

        isTurn = true;
        startingPosition = transform.position;
        StartCoroutine(EnemyTurnCoroutine());
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
    /// <returns>Waits for the duration of enemyTurnTime before ending turn.</returns>
    private IEnumerator EnemyTurnCoroutine()
    {
        PlayerCharacter target = FindNearestPlayer();
        if (target == null)
        {
            yield return new WaitForSeconds(0.5f);
            EndTurn();
            ActionManager.instance.NextTurn();
            yield break;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > attackRange)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);

            foreach (Vector3 corner in path.corners)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
                    break;

                while (Vector3.Distance(transform.position, corner) > 0.1f)
                {
                    Vector3 direction = (corner - transform.position).normalized;
                    Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, corner, moveSpeed * Time.deltaTime);

                    yield return null;
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            target.health -= attackDamage;

            yield return new WaitForSeconds(0.5f);
        }

        ActionManager.instance.NextTurn();
    }

    /// <summary>
    /// Returns the closest player to the enemy
    /// </summary>
    private PlayerCharacter FindNearestPlayer()
    {
        List<PlayerCharacter> players = PartyManager.instance.partyMembers.Where(p => p.isAlive).ToList();
        PlayerCharacter closest = null!;
        float minDistance = float.MaxValue;

        foreach (var player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                closest = player;
                minDistance = distance;
            }
        }

        return closest;
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
