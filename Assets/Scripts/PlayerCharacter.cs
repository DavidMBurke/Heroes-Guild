using System;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : Being
{
    public int actionPoints = 2;
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
    }

    public void StartMovementAction()
    {
        isInMovementAction = true;
        remainingMovement = maxMoveDistance;
        hasMovement = false;
        rangeIndicator.gameObject.SetActive(true);
    }

    public void EndMovementAction()
    {
        isInMovementAction = false;
        remainingMovement = 0;
        rangeIndicator.gameObject.SetActive(false);
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void MoveToClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return;
        }

        targetPosition = hit.point;
        targetPosition.y = transform.position.y;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (remainingMovement <= 0)
        {
            return;
        }
        if (distanceToTarget > remainingMovement)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            targetPosition = transform.position + direction * remainingMovement;
        }

        isMoving = true;
        rangeIndicator.gameObject.SetActive(false);

    }

    void Move()
    {
        Vector3 rangeScale = rangeIndicator.gameObject.transform.localScale;
        rangeScale = new Vector3(remainingMovement * 2, rangeScale.y, remainingMovement * 2);
        rangeIndicator.gameObject.transform.localScale = rangeScale;
        rangeIndicatorColor = new Color(.5f,.5f,.5f,0.1f);

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
}
