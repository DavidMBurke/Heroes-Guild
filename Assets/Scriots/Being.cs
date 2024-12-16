using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being : MonoBehaviour
{
    public float maxMoveDistance = 5f;

    public bool isTurn = false;
    public LayerMask groundLayer;
    public float moveSpeed = 5f;
    protected Vector3 targetPosition;
    protected bool isMoving = false;
    public GameObject turnIndicator;
    public Color characterColor;
    public Color turnIndicatorColor = new Color(.2f, 1f, .2f, .1f);
    public GameObject model;


    // Start is called before the first frame update
    protected void Start()
    {
        targetPosition = transform.position;
        
        MeshRenderer renderer = model.GetComponent<MeshRenderer>();
        renderer.material.color = characterColor;

        MeshRenderer turnIndicatorRenderer = turnIndicator.GetComponent<MeshRenderer>();
        turnIndicatorRenderer.material = new Material(turnIndicatorRenderer.material);
        turnIndicatorRenderer.material.color = turnIndicatorColor;
    }

    // Update is called once per frame
    protected void Update()
    {
        turnIndicator.SetActive(isTurn);
    }

    protected void FixedUpdate()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z = 0;
        rotation.x = 0;
        transform.eulerAngles = rotation;
    }
}
