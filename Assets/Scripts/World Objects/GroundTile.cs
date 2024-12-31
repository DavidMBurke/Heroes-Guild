using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public int group;
    public int x;
    public int z;

    void Start()
    {
        MeshRenderer tileRenderer = gameObject.GetComponent<MeshRenderer>();
        tileRenderer.material.color = Color.black;
    }

}
