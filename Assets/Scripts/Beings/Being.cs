using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Umbrella class for everything creature-like
/// </summary>
public class Being : MonoBehaviour
{
    // ========== Info ==========
    public string characterName = null!;

    public float moveDistance = 5f;
    public float moveSpeed = 5f;
    public bool isMoving = false; // To prevent mid-movement, probably will expand this to isInterruptable for any action that needs to complete
    public bool isInMovementAction = false; // For button logic
    protected Vector3 targetPosition;
    public Vector3 startingPosition;

    // ========== Action & Movement ========== TODO work these into enemy logic
    public int maxActionPoints = 1;
    public int actionPoints = 0;
    public bool hasMovement = true;
    public bool endMove = false;
    public bool dodgeEnabled = true;

    // ========== Core Systems ==========
    public Attributes attributes = new Attributes();
    public Affinities affinities = new Affinities();
    [SerializeField] public CombatSkills combatSkills = new CombatSkills();
    [SerializeField] public NonCombatSkills nonCombatSkills = new NonCombatSkills();
    [SerializeField] public EquipmentSlots equipmentSlots = new EquipmentSlots();
    public List<CharacterAction> actionList = new List<CharacterAction>();

    // ========== Action ==========
    public bool isTurn = false;
    public bool isInCharacterAction = false;
    public CharacterAction currentAction = null!;
    public float initiative;
    public bool isInScene = false;

    // ========== Stats ==========
    public int health = 100;
    public int maxHealth = 100;
    public bool isAlive = true;
    public int level = 1;

    // ========== Model ==========
    private Rigidbody rb = null!;
    public GameObject model = null!;
    public Color characterColor;

    // Indicators TODO - Move these to their own class and single object in scene
    public GameObject turnIndicator = null!;
    public GameObject rangeIndicator = null!;
    public Color rangeIndicatorColor;
    public Color rangeIndicatorCombatColor = new Color(.5f, 0, 0, .25f);
    public Color rangeIndicatorMovementColor = new Color(.5f, .5f, .5f, .25f);
    public Color turnIndicatorColor = new Color(0, 1f, 0, .2f);
    public float interactDistance = 1.5f;

    // ========== Inventory ==========
    public List<Item> inventory;

    /// <summary>
    /// Unity Start method. Initializes references and visuals if in scene.
    /// </summary>
    protected void Start()
    {
        targetPosition = transform.position;
        rb = GetComponentInChildren<Rigidbody>();

        if (!isInScene) return;

        ApplyColors();
    }

    /// <summary>
    /// Unity Update method. Refreshes status and visuals if in scene.
    /// </summary>
    protected void Update()
    {
        if (!isInScene) return;

        turnIndicator.SetActive(isTurn);
        ApplyColors();
    }

    public void ModifyHealth(int changeInHealth)
    {
        health += changeInHealth;
        Color textColor = changeInHealth > 0 ? Color.green : Color.red;
        ActionManager.instance.floatingTextSpawner.ShowText($"{changeInHealth}", transform.position, textColor);
        CheckStatus();
    }

    /// <summary>
    /// Checks the being's status (e.g. health) and updates if needed.
    /// </summary>
    private void CheckStatus()
    {
        if (health <= 0 && isAlive)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles death logic: disables physics constraints and adds torque to "knock over" the character.
    /// </summary>
    private void Die()
    {
        isAlive = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(new Vector3(0, 0, 1.5f), ForceMode.Impulse);
    }

    /// <summary>
    /// Called when values change in editor. Updates visual representation.
    /// </summary>
    protected void OnValidate()
    {
        if (!isInScene) return;

        ApplyColors();
    }

    /// <summary>
    /// Begins a coroutine-based character action.
    /// </summary>
    public void StartCharacterAction(CharacterAction action)
    {
        if (isInCharacterAction) EndCharacterAction();
        action.endSignal = false;
        currentAction = action;
        StartCoroutine(action.action(action.character, action));
    }

    /// <summary>
    /// Ends the currently executing action.
    /// </summary>
    public void EndCharacterAction()
    {
        isInCharacterAction = false;
        currentAction.EndAction();
    }

    /// <summary>
    /// Applies character color and transparency to model and indicators.
    /// </summary>
    private void ApplyColors()
    {
        MeshRenderer renderer = model.GetComponent<MeshRenderer>();
        MeshRenderer turnIndicatorRenderer = turnIndicator.GetComponent<MeshRenderer>();
        MeshRenderer rangeIndicatorRenderer = rangeIndicator.GetComponent<MeshRenderer>();

        if (Application.isPlaying)
        {
            turnIndicatorRenderer.material = new Material(Shader.Find("Transparent/Diffuse"));
            rangeIndicatorRenderer.material = new Material(Shader.Find("Transparent/Diffuse"));

            renderer.material.color = characterColor;
        }
        else if (renderer != null && renderer.sharedMaterial != null)
        {
            renderer.sharedMaterial.color = characterColor;
        }

        if (turnIndicatorRenderer?.sharedMaterial != null)
            turnIndicatorRenderer.sharedMaterial.color = turnIndicatorColor;

        if (rangeIndicatorRenderer?.sharedMaterial != null)
            rangeIndicatorRenderer.sharedMaterial.color = rangeIndicatorColor;
    }

    /// <summary>
    /// Unity FixedUpdate method. Locks vertical rotation and posture.
    /// </summary>
    protected void FixedUpdate()
    {
        if (!isInScene) return;

        FixVertical();
        //FixPosition(); // Optional: Re-enable if needed for fixing Y-axis offset
    }

    /// <summary>
    /// Locks the being's rotation to upright if alive.
    /// </summary>
    private void FixVertical()
    {
        if (!isAlive)
        {
            rb.constraints = RigidbodyConstraints.None;
            return;
        }

        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public bool AttemptAttackOnThisBeing(int toHit, int damage)
    {
        if (actionPoints > 0 && dodgeEnabled)
        {
            if (AttemptDodge(toHit))
            {
                return false;
            };
        }
        if (AttemptBlock(toHit))
        {
            return false;
        };
        ModifyHealth(-damage);
        return true;
    }

    public bool AttemptDodge(int toHit)
    {
        int toDodge = DiceRoller.Roll(1, 100, (int)combatSkills.GetSkill(CombatSkills.Enum.Dodge).modifiedLevel);
        Debug.Log($"{characterName} rolled {toDodge} to dodge.");
        if (toDodge >= toHit)
        {
            Debug.Log($"{characterName} dodged successfully.");
            ActionManager.instance.floatingTextSpawner.ShowText("Dodged!", transform.position, Color.black);
            return true;
        }
        return false;
    }
    public bool AttemptBlock(int toHit)
    {
        int toBlock = DiceRoller.Roll(1, 100, (int)combatSkills.GetSkill(CombatSkills.Enum.Block).modifiedLevel);
        Debug.Log($"{characterName} rolled {toBlock} to block.");
        if (toBlock >= toHit)
        {
            Debug.Log($"{characterName} blocked successfully.");
            ActionManager.instance.floatingTextSpawner.ShowText("Blocked!", transform.position, Color.black);
            return true;
        }
        return false;
    }
}
