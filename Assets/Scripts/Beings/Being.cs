using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Umbrella class for everything creature-like
/// </summary>
public class Being : MonoBehaviour
{
    // Info
    public string characterName;

    // Movement
    public float moveDistance = 5f;
    public float moveSpeed = 5f;
    public bool isMoving = false; // To prevent mid-movement, probably will expand this to isInterruptable for any action that needs to complete
    public bool isInMovementAction = false; // For button logic
    protected Vector3 targetPosition;
    public Vector3 startingPosition;

    // Action
    public bool isTurn = false;
    public bool isInCharacterAction = false;
    public CharacterAction currentAction;
    public float initiative;

    // Stats
    public int health = 100;
    public int maxHealth = 100;
    public bool isAlive = true;
    ActionManager actionManager;
    
    // Model
    private Rigidbody rb;
    public GameObject model;
    public Color characterColor;

    // Indicators TODO - Move these to their own class and single object in scene
    public GameObject turnIndicator;
    public GameObject rangeIndicator;
    public Color rangeIndicatorColor;
    public Color rangeIndicatorCombatColor = new Color(.5f, 0, 0, .25f);
    public Color rangeIndicatorMovementColor = new Color(.5f, .5f, .5f, .25f);
    public Color turnIndicatorColor = new Color(0, 1f, 0, .2f);
    public float interactDistance = 1.5f;

    // Inventory
    public List<Interactable> inventory;

    protected void Start()
    {
        actionManager = ActionManager.instance;
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

    /// <summary>
    /// Check status indicators
    /// </summary>
    private void checkStatus()
    {
        if (health <= 0 && isAlive)
        {
            die();
        }
    }

    /// <summary>
    /// Set isAlive to false, renove rotational lock and push over
    /// </summary>
    private void die()
    {
        isAlive = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(new Vector3(0, 0, 1.5f), ForceMode.Impulse);
    }

    protected void OnValidate()
    {
        ApplyColors();
    }

    /// <summary>
    /// Set colors and transparency of being, turn indicator and range indicator
    /// </summary>
    private void ApplyColors()
    {
        MeshRenderer renderer = model.GetComponent<MeshRenderer>();
        MeshRenderer turnIndicatorRenderer = turnIndicator.GetComponent<MeshRenderer>();
        MeshRenderer rangeIndicatorRenderer = rangeIndicator.GetComponent<MeshRenderer>();

        // Did this because sharedMaterial was causing the wrong items to get colored in playmode
        // but material was throwing errors for instantiating materials in edit mode
        if (Application.isPlaying)
        {
            turnIndicatorRenderer.material = new Material(Shader.Find("Transparent/Diffuse"));
            rangeIndicatorRenderer.material = new Material(Shader.Find("Transparent/Diffuse"));

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

    /// <summary>
    /// Set and lock being upright
    /// </summary>
    /// note: For some reason only doing one of locking rotation and setting to zero still allowed falling over but both do not
    private void FixVertical()
    {
        if (!isAlive)
        {
            rb.constraints = RigidbodyConstraints.None;
            return;
        }
        Vector3 rotation = transform.eulerAngles;
        rotation.z = 0;
        rotation.x = 0;
        transform.eulerAngles = rotation;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

}
