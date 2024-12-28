using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
    public int conwayMinNeighborsToComeAlive; // Neighbors = walls tiles adjacent or diagonal to
    public int conwayMaxNeighborsToComeAlive;
    public int conwayMinNeighborsToStayAlive;
    public int conwayMaxNeighborsToStayAlive;
    private bool canMoveDiagonal = false;//
    public Color emptyColor;
    public Color wallColor;

    public bool updatePerlinOnSettingsChange;
    public bool subtractPerlin;
    public bool addPerlin;
    [Range(0f, 1f)]
    public float perlinThreshold;
    [Range(0f, 1f)]
    public float perlinScaleFactor1;
    [Range(0f, 1f)]
    public float perlinScaleFactor2;
    [Range(0f, 1f)]
    public float dropOffRate;
    private float seedX1, seedZ1;
    private float seedX2, seedZ2;
    private List<Color> colorList = new List<Color>();
    [Range(1f, 20f)]
    public float wallHeight;


    private void Start()
    {
        updateColor();
        raiseWalls();
    }

    private void OnValidate()
    {
        Renderer renderer = tilePrefab.GetComponent<Renderer>();
        tileSize = renderer.bounds.size;
        updateColor();
        raiseWalls();
    }

    /// <summary>
    /// Generate x by z tile map with all impassable tiles
    /// </summary>
    public void GenerateBlankMap()
    {
        DestroyAllTiles();
        tileMap = new List<GroundTile>();
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                GroundTile tile = Instantiate(tilePrefab, transform);
                tile.transform.localPosition = new Vector3(x * tileSize.x, 0, z * tileSize.z);
                tile.x = x;
                tile.z = z;
                SetWall(tile);
                tileMap.Add(tile);
            }
        }
        UpdateEditorViews();
    }

    public void DestroyAllTiles()
    {
        if (tileMap != null)
        {
            foreach (GroundTile tile in tileMap)
            {
                if (tile != null && tile.gameObject != null)
                {
                    DestroyImmediate(tile.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Update scenes immediately outside of runtime
    /// </summary>
    private void UpdateEditorViews()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                int index = x * zCount + z;
                Renderer renderer = tileMap[index].GetComponent<Renderer>();
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                int group = tileMap[index].group;
                int timeOut = xCount * zCount;
                if (group >= 0)
                {
                    while (colorList.Count <= group && timeOut > 0)
                    {
                        colorList.Add(new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                        timeOut--;
                        if (timeOut <= 0)
                        {
                            Debug.LogWarning("Timed out on UpdateEditorViews");
                        }
                    }

                }
                switch (group)
                {
                    case -2: propertyBlock.SetColor("_Color", Color.white); break;
                    case -1: propertyBlock.SetColor("_Color", Color.black); break;
                    default: propertyBlock.SetColor("_Color", colorList[group]); break;
                }
                renderer.SetPropertyBlock(propertyBlock);
            }
        }
        SceneView.RepaintAll();
        EditorApplication.QueuePlayerLoopUpdate();
    }

    /// <summary>
    /// Set tile to passable
    /// </summary>
    /// <param name="tile"></param>
    public void SetEmpty(GroundTile tile)
    {
        tile.isWall = false;
        tile.group = -2;
    }

    /// <summary>
    /// Set tile to impassable
    /// </summary>
    /// <param name="tile"></param>
    public void SetWall(GroundTile tile)
    {
        tile.isWall = true;
        tile.group = -1;
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
                SetEmpty(tileMap[index]);
            }
        }

        UpdateEditorViews();
    }

    /// <summary>
    /// Generate two layers of perlin noise, with the second diminished by dropOffRate
    /// </summary>
    public void GeneratePerlinNoise()
    {
        if (tileMap == null) return;
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
                    SetEmpty(tileMap[index]);
                }
                if (noiseValue >= perlinThreshold && addPerlin)
                {
                    SetWall(tileMap[index]);
                }
            }
        }
        UpdateEditorViews();
    }

    /// <summary>
    /// Change seed for perlin noise generation
    /// </summary>
    public void Reseed()
    {
        seedX1 = Random.Range(0f, 10000f);
        seedZ1 = Random.Range(0f, 10000f);
        seedX2 = Random.Range(0f, 10000f);
        seedZ2 = Random.Range(0f, 10000f);
        GeneratePerlinNoise();
    }

    /// <summary>
    /// Add a border of walls around the map
    /// </summary>
    public void GenerateBorder()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                if (x < borderThickess || x >= (xCount - borderThickess) || z < borderThickess || z >= (zCount - borderThickess))
                {
                    int index = x * zCount + z;
                    SetWall(tileMap[index]);
                }
            }
        }
        UpdateEditorViews();
    }

    /// <summary>
    /// Apply conway game of life rules with adjustable neighbor counts.
    /// </summary>
    public void Conway()
    {
        List<bool> tileNewStateTracker = new List<bool>();
        foreach (GroundTile tile in tileMap)
        {
            tileNewStateTracker.Add(tile.isWall);
        }
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
                        if (tileMap[index].isWall)
                        {
                            neighbors++;
                        }
                        if (m == 0 && n == 0 && tileMap[index].isWall)
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
                if (tileMap[index].isWall != tileNewStateTracker[index])
                {
                    if (tileNewStateTracker[index])
                    {
                        SetWall(tileMap[index]);
                        continue;
                    }
                    SetEmpty(tileMap[index]);
                }
            }
        }

        UpdateEditorViews();
    }

    /// <summary>
    /// Find all contiguous groups, return list of group numbers and their counts
    /// </summary>
    public List<(int, int)> FindGroups()
    {
        List<(int, int)> groupCount = new List<(int, int)>();
        (int group, int count) = (0, 0);
        foreach (GroundTile tile in tileMap)
        {
            tile.group = tile.isWall ? -1 : -2; // -1 = wall, -2 = unassigned
        }
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                int index = x * zCount + z;
                if (tileMap[index].group != -2) continue;
                Queue<int> neighbors = new Queue<int>();
                neighbors.Enqueue(index);
                tileMap[index].group = group;
                int timeOut = xCount * zCount;
                while (neighbors.Count > 0 && timeOut > 0)
                {
                    int currentIndex = neighbors.Dequeue();

                    foreach (int neighborIndex in findNeighbors(currentIndex))
                    {
                        if (tileMap[neighborIndex].group == -2)
                        {
                            tileMap[neighborIndex].group = group;
                            count++;
                            neighbors.Enqueue(neighborIndex);
                        }
                    }
                    timeOut--;
                }
                groupCount.Add((group, count));
                count = 1;
                group++;
                if (timeOut <= 0)
                {
                    Debug.LogWarning("Timed out on FindGroups");
                }
            }
        }
        UpdateEditorViews();
        return groupCount;
    }



    /// <summary>
    /// Return indices of all neighboring cells
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<int> findNeighbors(int index)
    {
        List<int> neighbors = new List<int>();
        int x = index / zCount;
        int z = index % zCount;
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (!canMoveDiagonal && Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1) continue;
                    int neighborX = x + i;
                    int neighborZ = z + j;
                    if (neighborX >= 0 && neighborX < xCount && neighborZ >= 0 && neighborZ < zCount)
                    {
                        int neighborIndex = neighborX * zCount + neighborZ;
                        neighbors.Add(neighborIndex);
                    }
                }
            }
        }
        return neighbors;
    }

   public void randomWalkFromSmallestGroup()
    {
        List<(int, int)> groupsAndCounts = FindGroups();
        int smallestGroup = groupsAndCounts.OrderBy((g) => g.Item2).ToList()[0].Item1;
        List<GroundTile> groupTiles = tileMap.Where(t => t.group == smallestGroup).ToList();
        int index = Random.Range(0, groupTiles.Count);
        (int currentX, int currentZ) = (groupTiles[index].x, groupTiles[index].z);
        int nextGroup = smallestGroup;
        int timeOut = 1000;
        while ((nextGroup == smallestGroup || nextGroup < 0) && timeOut > 0)
        {
            int option = Random.Range(0, 4);
            (int, int)[] shift = { (0, -1), (-1, 0), (1, 0), (0, 1) };
            int nextX = currentX + shift[option].Item1;
            int nextZ = currentZ + shift[option].Item2;

            if (nextX < borderThickess || nextX >= (xCount - borderThickess) || nextZ < borderThickess || nextZ >= (zCount - borderThickess)) 
            {
                continue;
            }

            int i = nextX * zCount + nextZ;
            currentX = nextX;
            currentZ = nextZ;
            nextGroup = tileMap[i].group;
            SetEmpty(tileMap[i]);
            tileMap[i].group = smallestGroup;
            timeOut--;
            if (timeOut <= 0)
            {
                Debug.LogWarning("Timed out on randomWalkFromSmallestGroup");
            }
        }
        UpdateEditorViews();
    }

    public void generateRandomForestMap()
    {
        perlinThreshold = .5f;
        perlinScaleFactor1 = .15f;
        perlinScaleFactor2 = .85f;
        dropOffRate = .33f;
        addPerlin = true;
        subtractPerlin = true;
        conwayCanComeAlive = true;
        conwayCanDie = true;
        conwayMinNeighborsToComeAlive = 5;
        conwayMaxNeighborsToComeAlive = 8;
        conwayMinNeighborsToStayAlive = 3;
        conwayMaxNeighborsToStayAlive = 8;
        borderThickess = 1;
        GenerateBlankMap();
        Reseed();
        GeneratePerlinNoise();
        GenerateBorder();
        Conway();
        int groups = 10;
        int timeOut = 1000;
        while (groups > 1 & timeOut > 0) {
            randomWalkFromSmallestGroup();
            groups = FindGroups().Count;
            timeOut--;
            if (timeOut <= 0) {
                Debug.LogWarning("Timed out on generateRandomForestMap");
            }
        }
        UpdateEditorViews();
    }

    /// <summary>
    /// 
    /// </summary>
    public void raiseWalls()
    {
        Vector3 s = tilePrefab.transform.localScale;
        foreach (GroundTile tile in tileMap)
        {
            if (!tile.isWall) continue;
            tile.transform.localScale = new Vector3(s.x, s.y * wallHeight, s.z);
        }
    }

    /// <summary>
    /// Update map color 
    /// </summary>
    public void updateColor()
    {
        foreach (GroundTile tile in tileMap)
        {
            Renderer renderer = tile.GetComponent<Renderer>();
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            switch (tile.group)
            {
                case -2: propertyBlock.SetColor("_Color", Color.white); break;
                case -1: propertyBlock.SetColor("_Color", wallColor); break;
                default: propertyBlock.SetColor("_Color", emptyColor); break;
            }
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
