using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being : MonoBehaviour
{
    public float maxMoveDistance = 5f;
    public bool isTurn = false;
    public LayerMask groundLayer;
    public LayerMask attackableLayer;
    public float moveSpeed = 5f;
    protected Vector3 targetPosition;
    protected bool isMoving = false;
    public GameObject turnIndicator;
    public GameObject rangeIndicator;
    public Color rangeIndicatorColor;
    public Color rangeIndicatorCombatColor = Color.red;
    public Color rangeIndicatorMovementColor = Color.gray;
    public Color characterColor;
    public Color turnIndicatorColor = new Color(.2f, 1f, .2f, .1f);
    public GameObject model;
    public float remainingMovement = 0;
    public bool isInMovementAction = false;
    public Vector3 startingPosition;
    public int health = 100;
    public bool isInCharacterAction = false;
    public bool isAlive = true;
    private Rigidbody rb;
    public Attack selectedAttack; 


    // Start is called before the first frame update
    protected void Start()
    {
        targetPosition = transform.position;
        rb = GetComponentInChildren<Rigidbody>();
        ApplyColors();
    }

    protected void Update()
    {
        turnIndicator.SetActive(isTurn);
        ApplyColors();
        checkStatus();
    }

    private void checkStatus()
    {
        if (health <= 0 && isAlive)
        {
            die();
        }
    }

    private void die()
    {
        isAlive = false;
        rb.AddTorque(new Vector3(0, 0, 1.5f), ForceMode.Impulse);
    }

    protected void OnValidate()
    {
        ApplyColors();
    }

    private void ApplyColors()
    {
        MeshRenderer renderer = model.GetComponent<MeshRenderer>();
        MeshRenderer turnIndicatorRenderer = turnIndicator.GetComponent<MeshRenderer>();
        MeshRenderer rangeIndicatorRenderer = rangeIndicator.GetComponent<MeshRenderer>();

        // Did this because sharedMaterial was causing the wrong items to get colored in playmode
        // but material was throwing errors for instantiating materials in edit mode
        if (Application.isPlaying)
        {
            renderer.material.color = characterColor;
            turnIndicatorRenderer.material.color = turnIndicatorColor;
            rangeIndicatorRenderer.material.color = rangeIndicatorColor;
        } else {
            renderer.sharedMaterial.color = characterColor;
            turnIndicatorRenderer.sharedMaterial.color = turnIndicatorColor;
            rangeIndicatorRenderer.sharedMaterial.color = rangeIndicatorColor;
        }

    }

    protected void FixedUpdate()
    {
        FixVertical();
    }

    private void FixVertical()
    {
        if (!isAlive)
        {
            return;
        }
        Vector3 rotation = transform.eulerAngles;
        rotation.z = 0;
        rotation.x = 0;
        transform.eulerAngles = rotation;
    }

}
