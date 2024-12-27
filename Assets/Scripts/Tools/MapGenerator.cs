using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using UnityEditor;

/// <summary>
/// Generate maps
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public GroundTile tilePrefab;
    public List<GroundTile> tileMap;
    public int xCount = 10;
    public int zCount = 10;
    private Vector3 tileSize;
    public int roomCorner1x, roomCorner1z;
    public int roomCorner2x, roomCorner2z;
    public int borderThickess;
    public bool conwayCanComeAlive;
    public bool conwayCanDie;
    public int conwayMinNeighborsToComeAlive; // Neighbors = full tiles adjacent or diagonal to
    public int conwayMaxNeighborsToComeAlive;
    public int conwayMinNeighborsToStayAlive;
    public int conwayMaxNeighborsToStayAlive;
    private List<bool> tileIsFullTracker;
    private float seedX1, seedZ1;
    private float seedX2, seedZ2;
    MaterialPropertyBlock emptyPropertyBlock;
    MaterialPropertyBlock fullPropertyBlock;

    private void OnValidate()
    {
        if (tileMap == null || tileIsFullTracker == null || tileMap.Count != tileIsFullTracker.Count)
        {
            GenerateBlankMap();
        }
        Renderer renderer = tilePrefab.GetComponent<Renderer>();
        tileSize = renderer.bounds.size;
        emptyPropertyBlock = new MaterialPropertyBlock();
        fullPropertyBlock = new MaterialPropertyBlock();
        emptyPropertyBlock.SetColor("_Color", UnityEngine.Color.white);
        fullPropertyBlock.SetColor("_Color", UnityEngine.Color.black);
        GeneratePerlinNoise();
        GenerateBorder();
    }

    /// <summary>
    /// Generate x by z tile map with all impassable tiles
    /// </summary>
    public void GenerateBlankMap()
    {
        foreach (GroundTile tile in tileMap)
        {
            DestroyImmediate(tile);
        }
        tileMap = new List<GroundTile>();
        tileIsFullTracker = new List<bool>();
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                int index = x * zCount + z;
                GroundTile tile = Instantiate(tilePrefab, transform);
                tile.transform.localPosition = new Vector3(x * tileSize.x, 0, z * tileSize.z);
                tileIsFullTracker.Add(true);
                SetFull(tile, index);
                tileMap.Add(tile);
            }
        }
    }

    /// <summary>
    /// Set tile to passable
    /// </summary>
    /// <param name="tile"></param>
    public void SetEmpty(GroundTile tile, int index)
    {
        if (index > -1)
        {
            tileIsFullTracker[index] = false;
        } else
        {
            Debug.Log("Error on SetEmpty");
        }
        Renderer renderer = tile.GetComponent<Renderer>();
        renderer.SetPropertyBlock(emptyPropertyBlock);
    }

    /// <summary>
    /// Set tile to impassable
    /// </summary>
    /// <param name="tile"></param>
    public void SetFull(GroundTile tile, int index)
    {
        if (index > -1)
        {
            tileIsFullTracker[index] = true;
        } else
        {
            Debug.Log("Error on SetFull");
        }
        Renderer renderer = tile.GetComponent<Renderer>();
        renderer.SetPropertyBlock(fullPropertyBlock);
    }

    /// <summary>
    /// Draw a rectangle of clear tiles from corner to corner
    /// </summary>
    public void GenerateRoom()
    {
        for (int x = roomCorner1x; x < roomCorner2x; x++)
        {
            for (int z = roomCorner1z; z < roomCorner2z; z++) 
            {
                int index = x * zCount + z;
                SetEmpty(tileMap[index], index);
            }
        }

        UpdateEditorViews();
    }

    public bool updatePerlinOnSettingsChange;
    public bool subtractPerlin;
    public bool addPerlin;
    [Range(0f,1f)] 
    public float perlinThreshold;
    [Range(0f, 1f)]
    public float perlinScaleFactor1;
    [Range(0f, 1f)]
    public float perlinScaleFactor2;
    [Range(0f, 1f)]
    public float dropOffRate;
    public void GeneratePerlinNoise()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                int index = x * zCount + z;
                float xIn1 = perlinScaleFactor1 * x + seedX1;
                float zIn1 = perlinScaleFactor1 * z + seedZ1;
                float xIn2 = perlinScaleFactor2 * x + seedX2;
                float zIn2 = perlinScaleFactor2 * z + seedZ2;
                float noiseValue1 = Mathf.PerlinNoise(xIn1, zIn1);
                float noiseValue2 = Mathf.PerlinNoise(xIn2, zIn2) - .5f;
                float noiseValue = noiseValue1 + noiseValue2 * dropOffRate;
                if (noiseValue < perlinThreshold && subtractPerlin)
                {
                    SetEmpty(tileMap[index], index);
                }
                if (noiseValue >= perlinThreshold && addPerlin)
                {
                    SetFull(tileMap[index], index);
                }
            }
        }
        UpdateEditorViews();
    }

    private void UpdateEditorViews()
    {
        SceneView.RepaintAll();
        EditorApplication.QueuePlayerLoopUpdate();
    }

    public void Reseed()
    {
        seedX1 = Random.Range(0f, 10000f);
        seedZ1 = Random.Range(0f, 10000f);
        seedX2 = Random.Range(0f, 10000f);
        seedZ2 = Random.Range(0f, 10000f);
        GeneratePerlinNoise();
    }

    public void GenerateBorder()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                if (x < borderThickess || x >= (xCount - borderThickess) || z < borderThickess || z >= (zCount - borderThickess))
                {
                    int index = x * zCount + z;
                    SetFull(tileMap[index], index);
                }
            }
        }
        UpdateEditorViews();
    }

    public void Conway()
    {
        List<bool> tileNewStateTracker = new List<bool>(tileIsFullTracker);
        for (int x = 1; x < xCount - 1; x++)
        {
            for (int z = 1; z < zCount - 1; z++)
            {
                int neighbors = 0;
                bool thisCellAlive = false;
                for (int m = -1; m < 2; m++)
                {
                    for (int n = -1; n < 2; n++) // Ogres are like onions
                    {
                        int index = (x + m) * zCount + (z + n);
                        if (tileIsFullTracker[index])
                        {
                            neighbors++;
                        }
                        if (m == 0 && n == 0 && tileIsFullTracker[index])
                        {
                            neighbors--;
                            thisCellAlive = true;
                        }
                    }
                }
                if (thisCellAlive == false &&
                    conwayCanComeAlive &&
                    neighbors >= conwayMinNeighborsToComeAlive && 
                    neighbors <= conwayMaxNeighborsToComeAlive)
                {
                    tileNewStateTracker[x * zCount + z] = true;
                    continue;
                }
                if (thisCellAlive == true &&
                    neighbors >= conwayMinNeighborsToStayAlive &&
                    neighbors <= conwayMaxNeighborsToStayAlive)
                {
                    tileNewStateTracker[x * zCount + z] = true;
                    continue;
                }
                if (conwayCanDie)
                {
                    tileNewStateTracker[x * zCount + z] = false;   
                }
            }
        }

        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                int index = x * zCount + z;
                if (tileIsFullTracker[index] != tileNewStateTracker[index])
                {
                    if (tileNewStateTracker[index])
                    {
                        SetFull(tileMap[index], index);
                        continue;
                    }
                    SetEmpty(tileMap[index], index);
                }
            }
        }


        tileIsFullTracker = tileNewStateTracker;

        UpdateEditorViews();
    }
}
