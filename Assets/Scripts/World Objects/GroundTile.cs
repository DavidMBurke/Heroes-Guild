using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public bool isFull;
    public int group;

    void Start()
    {
        MeshRenderer tileRenderer = gameObject.GetComponent<MeshRenderer>();
        tileRenderer.material.color = Color.black;
    }

}
