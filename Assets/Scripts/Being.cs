using UnityEngine;

public class Being : MonoBehaviour
{
    public float moveDistance = 5f;
    public bool isTurn = false;
    public float moveSpeed = 5f;
    protected Vector3 targetPosition;
    public bool isMoving = false;
    public GameObject turnIndicator;
    public GameObject rangeIndicator;
    public Color rangeIndicatorColor;
    public Color rangeIndicatorCombatColor = new Color(.5f, 0, 0, .25f);
    public Color rangeIndicatorMovementColor = new Color(.5f, .5f, .5f, .25f);
    public Color characterColor;
    public Color turnIndicatorColor = new Color(0, 1f, 0, .2f);
    public GameObject model;
    public bool isInMovementAction = false;
    public Vector3 startingPosition;
    public int health = 100;
    public bool isInCharacterAction = false;
    public bool isAlive = true;
    private Rigidbody rb;
    public CharacterAction currentAction;

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
