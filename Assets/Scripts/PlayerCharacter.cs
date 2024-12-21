using UnityEngine;

public class PlayerCharacter : Being
{
    public int maxActionPoints = 1;
    public int actionPoints = 0;
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
        actionPoints = maxActionPoints;
    }

    public void EndTurn()
    {
        isTurn = false;
        hasMovement = false;
        if (isInMovementAction)
        {
            EndMovementAction();
        }
        if (isInCharacterAction)
        {
            EndCharacterAction();
        }
    }

    public void StartCharacterAction(CharacterAction action)
    {
        if (isInMovementAction)
        {
            EndMovementAction();
        }
        StartCoroutine(action.actionFunction(action.character));
        isInCharacterAction = true;
    }

    public void EndCharacterAction()
    {
        isInCharacterAction = false;
    }

    public void StartMovementAction()
    {
        if (isInCharacterAction)
        {
            EndCharacterAction();
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

    void MoveToClick()
    {

        UIManager.CheckForUIElement();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
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

        if (Input.GetMouseButtonDown(0) & !isMoving)
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
}
