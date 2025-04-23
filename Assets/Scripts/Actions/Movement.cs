using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Movement actions to be passed as coroutines
/// </summary>
public class Movement
{
    /// <summary>
    /// Show range indicator if in turn-based mode, and move a character to position if within movement range, or indefinitely if free mode
    /// </summary>
    /// TODO --- Indication of accessible areas on move indicator
    /// <param name="being"></param>
    /// <param name="action"></param>
    /// <returns></returns>
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
            while ((remainingMovement > 1f || actionManager.IsFreeMode()) && player.endMove == false && action.endSignal == false)
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
                            ActionManager.instance.StartCoroutine(MoveFollower(follower, followerTarget, player.moveSpeed));
                        }

                        foreach (Vector3 corner in path.corners)
                        {
                            while (Vector3.Distance(player.transform.position, corner) > 1f && player.isMoving)
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
                        }
                    } else
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
    }

    /// <summary>
    /// Check if a point can be moved to, return that point's position if it can or the player's current position if not.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="remainingMovement"></param>
    /// <returns></returns>
    public static Vector3 MoveToClick(PlayerCharacter player, float remainingMovement)
    {
        if (UIManager.CheckForUIElement())
        {
            Debug.Log("Cannot move to target location due to clicking UI Element");
            return player.startingPosition;
        };
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
            if ((remainingMovement > 0 && Vector3.Distance(player.transform.position, targetPosition) <= remainingMovement) || ActionManager.instance.IsFreeMode())
            {
                targetPosition.y = player.transform.position.y;
                return targetPosition;
            }
            Debug.Log("Cannot move to target location: Not enough movement");
        }

        return player.startingPosition;

    }

    /// <summary>
    /// Return player to starting position and end move action
    /// </summary>
    /// <param name="player"></param>
    public static void UndoMove(PlayerCharacter player)
    {
        player.transform.position = player.startingPosition;
        player.rangeIndicator.gameObject.SetActive(false);
    }

}
