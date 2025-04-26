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

                        foreach (PlayerCharacter follower in PartyManager.instance.movementGroup)
                        {
                            Vector3 followerTarget = targetPosition + (follower.transform.position - player.transform.position).normalized * 1.5f;
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
}
