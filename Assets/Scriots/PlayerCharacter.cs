using System;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : Being
{

    new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
        if (!isTurn)
        {
            return; 
        }
        Move();
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

        if (distanceToTarget > maxMoveDistance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            targetPosition = transform.position + direction * maxMoveDistance;
        }

        isMoving = true;
        
        

    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToClick();
        }
        
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }


    }
}
