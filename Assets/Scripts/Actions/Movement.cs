using System.Collections;
using UnityEngine;

public class Movement
{
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
            bool isMoving = false;
            while ((remainingMovement > 0 || actionManager.IsFreeMode()) && player.endMove == false && action.endSignal == false)
            {
                player.rangeIndicator.gameObject.transform.localScale = new Vector3(remainingMovement * 2, scale.y, remainingMovement * 2);
                player.rangeIndicator.gameObject.SetActive(actionManager.IsTurnBasedMode());
                if (Input.GetMouseButtonDown(0))
                {
                    if (isMoving || UIManager.CheckForUIElement())
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
                    isMoving = true;
                }
                if (isMoving)
                {
                    player.rangeIndicator.gameObject.SetActive(false);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, player.moveSpeed * Time.deltaTime);
                    if (actionManager.IsTurnBasedMode())
                    {
                        remainingMovement -= player.moveSpeed * Time.deltaTime;
                    }

                    if (Vector3.Distance(player.transform.position, targetPosition) < 0.001f)
                    {
                        isMoving = false;
                        player.rangeIndicator.gameObject.SetActive(actionManager.IsTurnBasedMode());
                    }
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
            yield break;
        }
    }

    public static Vector3 MoveToClick(PlayerCharacter player, float remainingMovement)
    {
        UIManager.CheckForUIElement();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            return player.startingPosition;
        }
        Vector3 targetPosition = hit.point;
        targetPosition.y = player.transform.position.y;

        float distanceToTarget = Vector3.Distance(player.transform.position, targetPosition);

        if ((remainingMovement > 0 && distanceToTarget <= remainingMovement) || ActionManager.instance.IsFreeMode())
        {
            return targetPosition;
        }
        return player.startingPosition;

    }

    public static void UndoMove(PlayerCharacter player)
    {
        player.transform.position = player.startingPosition;
        player.rangeIndicator.gameObject.SetActive(false);
    }

}
