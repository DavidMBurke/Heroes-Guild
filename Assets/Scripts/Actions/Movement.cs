using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles movement actions as coroutines, supporting both turn-based and free movement modes.
/// </summary>
public class Movement
{
    /// <summary>
    /// Displays a movement range indicator (if turn-based), and moves the character to a valid clicked position if within movement range or allowed by free mode.
    /// </summary>
    /// <param name="being">The character being moved.</param>
    /// <param name="action">The action wrapper used to control execution.</param>
    /// <returns>An IEnumerator coroutine.</returns>
    public static IEnumerator Move(Being being, CharacterAction action)
    {
        if (being is PlayerCharacter player)
        {
            ActionManager actionManager = ActionManager.instance;
            float remainingMovement = player.moveDistance;
            player.isInMovementAction = true;
            Vector3 scale = player.rangeIndicator.gameObject.transform.localScale;
            Vector3 targetPosition = player.startingPosition;

            player.rangeIndicatorColor = player.rangeIndicatorMovementColor;
            player.isMoving = false;
            player.endMove = false;
            action.endSignal = false;
            player.startingPosition = player.transform.position;

            while ((remainingMovement > 1f || actionManager.IsFreeMode()) && !player.endMove && !action.endSignal)
            {
                player.rangeIndicator.gameObject.transform.localScale = new Vector3(remainingMovement * 2, scale.y, remainingMovement * 2);
                player.rangeIndicator.gameObject.SetActive(actionManager.IsTurnBasedMode());

                if (Input.GetMouseButtonDown(0))
                {
                    if ((player.isMoving && actionManager.IsTurnBasedMode()) || UIManager.CheckForUIElement())
                    {
                        yield return null;
                        continue;
                    }

                    targetPosition = MoveToClick(player, remainingMovement);
                    if (targetPosition == player.startingPosition)
                    {
                        yield return null;
                        continue;
                    }

                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(player.transform.position, targetPosition, NavMesh.AllAreas, path))
                    {
                        player.rangeIndicator.gameObject.SetActive(false);
                        player.isMoving = true;
                        Queue<Vector3> followerPositions = FindNearestUnoccupiedSpaces(player, targetPosition, skipCenter: true, spacesToFind: PartyManager.instance.movementGroup.Count);
                        foreach (PlayerCharacter follower in PartyManager.instance.movementGroup)
                        {
                            Vector3 followerTarget = followerPositions.Dequeue();
                            PartyManager.instance.StartFollowerMovement(follower, MoveFollower(follower, followerTarget, player.moveSpeed));
                        }

                        foreach (Vector3 corner in path.corners)
                        {
                            while (Vector3.Distance(player.transform.position, corner) > 0.01f && player.isMoving && !player.endMove && !action.endSignal)
                            {
                                Vector3 direction = (corner - player.transform.position).normalized;
                                if (direction != Vector3.zero)
                                {
                                    Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                                    player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, 360 * Time.deltaTime);
                                }

                                player.transform.position = Vector3.MoveTowards(player.transform.position, corner, player.moveSpeed * Time.deltaTime);

                                if (actionManager.IsTurnBasedMode())
                                {
                                    remainingMovement -= player.moveSpeed * Time.deltaTime;
                                }

                                yield return null;
                            }
                            if (player.endMove || action.endSignal)
                            {
                                foreach (PlayerCharacter follower in PartyManager.instance.movementGroup)
                                {
                                    PartyManager.instance.StopFollowerMovement(follower);
                                }
                            }
                        }

                    }
                    else
                    {
                        Debug.LogWarning("Path is invalid or incomplete");
                        yield return null;
                    }

                    player.isMoving = false;
                }

                if (Input.GetMouseButtonDown(1) && actionManager.IsTurnBasedMode())
                {
                    UndoMove(player);
                    break;
                }

                yield return null;
            }

            player.rangeIndicator.gameObject.SetActive(false);
            if (player.transform.position != player.startingPosition)
            {
                player.startingPosition = player.transform.position;
                player.hasMovement = false;
            }

            player.endMove = false;
            player.isInMovementAction = false;
        }
    }

    /// <summary>
    /// Moves a follower character along a path to the target position.
    /// </summary>
    /// <param name="follower">The follower character.</param>
    /// <param name="targetPosition">Target world position.</param>
    /// <param name="speed">Movement speed.</param>
    /// <returns>An IEnumerator coroutine.</returns>
    private static IEnumerator MoveFollower(PlayerCharacter follower, Vector3 targetPosition, float speed)
    {
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(follower.transform.position, targetPosition, NavMesh.AllAreas, path))
        {
            Debug.LogWarning($"Follower {follower.name} path invalid.");
            yield break;
        }

        foreach (Vector3 corner in path.corners)
        {
            while (Vector3.Distance(follower.transform.position, corner) > 0.5f)
            {
                Vector3 direction = (corner - follower.transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                    follower.transform.rotation = Quaternion.RotateTowards(follower.transform.rotation, toRotation, 360 * Time.deltaTime);
                }

                follower.transform.position = Vector3.MoveTowards(follower.transform.position, corner, speed * Time.deltaTime);
                yield return null;
            }
        }

        PartyManager.instance.followerMovementCoroutines.Remove(follower);
    }

    /// <summary>
    /// Gets the world position clicked by the player if it's a valid movement target.
    /// </summary>
    /// <param name="player">The player character.</param>
    /// <param name="remainingMovement">Remaining movement distance.</param>
    /// <returns>The target position or the starting position if invalid.</returns>
    public static Vector3 MoveToClick(PlayerCharacter player, float remainingMovement)
    {
        if (UIManager.CheckForUIElement())
        {
            Debug.Log("Cannot move to target location due to clicking UI Element");
            return player.startingPosition;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Cannot move to target location due to incorrect layer mask");
            return player.startingPosition;
        }

        Vector3 targetPosition = hit.point;
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            targetPosition = navMeshHit.position;

            if ((remainingMovement > 0 && Vector3.Distance(player.transform.position, targetPosition) <= remainingMovement) ||
                ActionManager.instance.IsFreeMode())
            {
                targetPosition.y = player.transform.position.y;
                return targetPosition;
            }

            Debug.Log("Cannot move to target location: Not enough movement");
        }

        return player.startingPosition;
    }

    /// <summary>
    /// Returns the player to their starting position and disables the range indicator.
    /// </summary>
    /// <param name="player">The player character.</param>
    public static void UndoMove(PlayerCharacter player)
    {
        player.transform.position = player.startingPosition;
        player.rangeIndicator.gameObject.SetActive(false);
    }

    public static Queue<Vector3> FindNearestUnoccupiedSpaces(Being being, Vector3 targetPostion, bool skipCenter = false, float startRadius = 0f, float maxSearchRadius = 10f, float angleIncrement = 60f, float radiusIncrement = 1f, int spacesToFind = 1)
    {
        Queue<Vector3> unoccupiedSpaces = new();
        if (spacesToFind <= 0) return unoccupiedSpaces;
        Collider moverCollider = being.GetComponentInChildren<Collider>();
        if (moverCollider == null) {
            Debug.LogWarning("Mover does not have a collider!");
            unoccupiedSpaces.Enqueue(targetPostion);
            return unoccupiedSpaces;
        }
        Vector3 searchCenter = targetPostion;
        float searchRadius = moverCollider.bounds.extents.magnitude * 1.5f;

        int layerMask = 1 << LayerMask.NameToLayer("Attackable");

        while (searchRadius <= maxSearchRadius && unoccupiedSpaces.Count < spacesToFind)
        {
            CapsuleCollider capsule = being.GetComponentInChildren<CapsuleCollider>();
            float checkRadius = capsule != null ? capsule.radius * 1.5f : 1f;
            if (!skipCenter && !Physics.CheckSphere(searchCenter, checkRadius, layerMask, QueryTriggerInteraction.Ignore))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(being.transform.position, searchCenter, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    unoccupiedSpaces.Enqueue(searchCenter);
                }
            }

            for (float angle = 0; angle < 360f; angle += angleIncrement)
            {

                if (unoccupiedSpaces.Count >= spacesToFind)
                {
                    return unoccupiedSpaces;
                }
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * searchRadius;
                Vector3 testPosition = searchCenter + offset;

                if (!Physics.CheckSphere(testPosition, checkRadius, layerMask, QueryTriggerInteraction.Ignore))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(being.transform.position, testPosition, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        unoccupiedSpaces.Enqueue(testPosition);
                    }
                }
            }

            searchRadius += radiusIncrement;
        }

        while (unoccupiedSpaces.Count <= spacesToFind)
        {
            Debug.Log($"Could not find a space for one of {being.characterName}'s followers");
            unoccupiedSpaces.Enqueue(targetPostion);
        }

        return unoccupiedSpaces;
    }

    /// <summary>
    /// Automatic movement for non player characters
    /// </summary>
    /// <param name="being"></param>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerator MoveToTarget(Being being, Vector3 destination, float speed, CharacterAction action)
    {
        NavMeshPath path = new();
        if (!NavMesh.CalculatePath(being.transform.position, destination, NavMesh.AllAreas, path))
        {
            Debug.LogWarning("Enemy Path Invalid");
            yield break;
        }

        being.isMoving = true;
        float remainingDistance = being.moveDistance;
        Vector3 currentPos = being.transform.position;

        foreach (Vector3 corner in path.corners)
        {
            while (Vector3.Distance(being.transform.position, corner) > 0.01f && !action.endSignal)
            {
                float step = speed * Time.deltaTime;

                float moveDistanceThisFrame = Mathf.Min(step, Vector3.Distance(being.transform.position, corner));
                if (moveDistanceThisFrame > remainingDistance)
                {
                    Vector3 direction = (corner - being.transform.position).normalized;
                    being.transform.position += direction * remainingDistance;
                    being.isMoving = false;
                    yield break;
                }

                remainingDistance -= moveDistanceThisFrame;

                Vector3 directionStep = (corner - being.transform.position).normalized;
                if (directionStep != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(directionStep, Vector3.up);
                    being.transform.rotation = Quaternion.RotateTowards(being.transform.rotation, toRotation, 360 * Time.deltaTime);
                }

                being.transform.position = Vector3.MoveTowards(being.transform.position, corner, step);
                yield return null;
            }

            if (remainingDistance <= 0 || action.endSignal)
            {
                being.hasMovement = false;
                being.isMoving = false;
                break;
            }
        }

        being.hasMovement = false;
        being.isMoving = false;
    }

    
}
