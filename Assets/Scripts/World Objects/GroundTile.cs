using UnityEngine;

public class GroundTile : MonoBehaviour
{
    void Start()
    {
        MeshRenderer tileRenderer = gameObject.GetComponent<MeshRenderer>();
        tileRenderer.material.color = Color.black;
    }

}
