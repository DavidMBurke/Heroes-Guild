using System;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : Being
{
    public int actionPoints = 1;
    public bool hasMovement = false;
    new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
        if (isTurn)
        {
            if (isInMovementAction)
            {
                Move();
            }
            if (isInCombatAction)
            {
                Combat();
            }
        }
    }

    public void StartTurn()
    {
        isTurn = true;
        hasMovement = true;
        startingPosition = transform.position;
    }

    public void EndTurn()
    {
        isTurn = false;
        hasMovement = false;
        if (isInMovementAction)
        {
            EndMovementAction();
        }
        if (isInCombatAction)
        {
            EndCombatAction();
        }
    }

    public void StartCombatAction()
    {
        if (isInMovementAction)
        {
            EndMovementAction();
        }
        isInCombatAction = true;
        rangeIndicator.gameObject.SetActive(true);
        rangeIndicatorColor = rangeIndicatorCombatColor;
    }

    public void EndCombatAction()
    {
        isInCombatAction = false;
        rangeIndicator.gameObject.SetActive(false);
    }

    public void StartMovementAction()
    {
        if (isInCombatAction)
        {
            EndCombatAction();
        }
        isInMovementAction = true;
        remainingMovement = maxMoveDistance;
        rangeIndicator.gameObject.SetActive(true);
        rangeIndicatorColor = rangeIndicatorMovementColor;
        startingPosition = transform.position;
    }

    public void EndMovementAction()
    {
        if (transform.position != startingPosition)
        {
            hasMovement = false;
        }
        isInMovementAction = false;
        remainingMovement = 0;
        rangeIndicator.gameObject.SetActive(false);
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void AttackClickedBeing()
    {
        Debug.Log("Attack Attempt");
        CheckForUIElement();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, attackableLayer)) 
        {
            return;
        }

        if (hit.collider.gameObject != this)
        {
            Debug.Log(hit.collider.gameObject);
        }
    }

    void Combat()
    {
        Vector3 scale = rangeIndicator.gameObject.transform.localScale;
        rangeIndicator.gameObject.transform.localScale = new Vector3(attackRange * 2, scale.y, attackRange * 2);
        if (Input.GetMouseButtonDown(0))
        {
            AttackClickedBeing();
        }

    }

    void MoveToClick()
    {
        CheckForUIElement();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return;
        }

        targetPosition = hit.point;
        targetPosition.y = transform.position.y;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (remainingMovement <= 0 || distanceToTarget > remainingMovement)
        {
            return;
        }
        //// Still deciding if I want the player to be able to move by clicking outside the move area
        //if (distanceToTarget > remainingMovement)
        //{
        //    Vector3 direction = (targetPosition - transform.position).normalized;
        //    targetPosition = transform.position + direction * remainingMovement;
        //}

        isMoving = true;
        rangeIndicator.gameObject.SetActive(false);

    }

    void Move()
    {
        Vector3 scale = rangeIndicator.gameObject.transform.localScale;
        rangeIndicator.gameObject.transform.localScale = new Vector3(remainingMovement * 2, scale.y, remainingMovement * 2);

        if (Input.GetMouseButtonDown(0))
        {
            MoveToClick();
        }
        
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            remainingMovement -= moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                isMoving = false;
                rangeIndicator.gameObject.SetActive(true);
            }
        }

    }
    public void UndoMove()
    {
        transform.position = startingPosition;
        remainingMovement = maxMoveDistance;
    }

    public void CheckForUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }
}
