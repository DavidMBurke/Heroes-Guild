using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Unity.AI.Navigation;

/// <summary>
/// Generate maps
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public GroundTile tilePrefab;
    public Enemy enemyPrefab;
    public List<GroundTile> tileMap;
    public NavMeshSurface navMeshSurface;
    public int xCount = 10;
    public int zCount = 10;
    private Vector3 tileSize;
    public int roomCorner1x, roomCorner1z;
    public int roomCorner2x, roomCorner2z;
    public int circleX, circleZ;
    public float radius;
    public int borderThickess;
    public bool conwayCanComeAlive;
    public bool conwayCanDie;
    public int conwayMinNeighborsToComeAlive; // Neighbors = walls tiles adjacent or diagonal to
    public int conwayMaxNeighborsToComeAlive;
    public int conwayMinNeighborsToStayAlive;
    public int conwayMaxNeighborsToStayAlive;
    private bool canMoveDiagonal = false;
    public Color emptyColor;
    public Color wallColor;
    public int playerSpawnX;
    public int playerSpawnZ;
    public float playerSpawnRadius;
    public int enemyCellSize;
    public int minCellSize;

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

    /// <summary>
    /// Clear map by destroying all tile gameObjects
    /// </summary>
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
                int index = GetIndex(x,z);
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
                    case -3: propertyBlock.SetColor("_Color", Color.green); break;
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
    /// Set tile to ground layer
    /// </summary>
    /// <param name="tile"></param>
    public void SetEmpty(GroundTile tile)
    {
        tile.group = -2;
        tile.gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    /// <summary>
    /// Set tile to impassable
    /// </summary>
    /// <param name="tile"></param>
    public void SetWall(GroundTile tile)
    {
        tile.group = -1;
        tile.gameObject.layer = LayerMask.NameToLayer("Wall");
    }

    /// <summary>
    /// Draw a rectangle of clear tiles from corner to corner
    /// </summary>
    public void ClearRect()
    {
        for (int x = roomCorner1x; x < roomCorner2x; x++)
        {
            for (int z = roomCorner1z; z < roomCorner2z; z++) 
            {
                int index = GetIndex(x,z);
                SetEmpty(tileMap[index]);
            }
        }

        UpdateEditorViews();
    }

    /// <summary>
    /// Create a circle of empty tiles
    /// </summary>
    public void ClearCircle(int group = -2)
    {

        for (int m = -(int)radius; m <= (int)radius; m++)
        {
            for (int n =  -(int)radius; n <= (int)radius; n++)
            {
                int x = circleX + m;
                int z = circleZ + n;
                if (dimensionsOutOfBounds(x, z)) 
                {
                    continue;
                }
                int index = GetIndex(x,z);
                if ((m*m + n*n) <= (radius*radius)) 
                {
                    if (group == -2)
                    {
                        SetEmpty(tileMap[index]);
                        continue;
                    }
                    tileMap[index].group = group;
                }
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
                int index = GetIndex(x,z);
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
                if (dimensionsOutOfBounds(x, z))
                {
                    int index = GetIndex(x,z);
                    SetWall(tileMap[index]);
                }
            }
        }
        UpdateEditorViews();
    }

    private bool dimensionsOutOfBounds(int x, int z)
    {
        return (x < borderThickess || x >= (xCount - borderThickess) || z < borderThickess || z >= (zCount - borderThickess));
    }

    /// <summary>
    /// Apply conway game of life rules with adjustable neighbor counts.
    /// </summary>
    public void Conway()
    {
        List<bool> tileNewStateTracker = new List<bool>();
        foreach (GroundTile tile in tileMap)
        {
            tileNewStateTracker.Add(tile.group == -1);
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
                        if (tileMap[index].group == -1)
                        {
                            neighbors++;
                        }
                        if (m == 0 && n == 0 && tileMap[index].group == -1)
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
                int index = GetIndex(x,z);
                if (tileMap[index].group == -1 != tileNewStateTracker[index])
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
    public List<(int, int)> FindGroups(bool makeSpawnCells = false)
    {
        List<(int, int)> groupCount = new List<(int, int)>();
        (int group, int count) = (0, 0);
        foreach (GroundTile tile in tileMap)
        {
            if (tile.group == -3)
            {
                tile.gameObject.layer = LayerMask.NameToLayer("Ground");
                continue;
            }
            tile.group = tile.group == -1 ? -1 : -2; // -1 = wall, -2 = unassigned
        }
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                int index = GetIndex(x,z);
                if (tileMap[index].group != -2) continue;
                Queue<int> neighbors = new Queue<int>();
                neighbors.Enqueue(index);
                tileMap[index].group = group;
                int timeOut = xCount * zCount;
                int cellsCount = enemyCellSize;
                while (neighbors.Count > 0 && timeOut > 0 && (cellsCount > 0 || !makeSpawnCells))
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
                    if (makeSpawnCells)
                    {
                        cellsCount--;
                    }
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

    /// <summary>
    /// Find smallest group and carve empty spaces in random cardinal directions until another group is connected
    /// </summary>
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

    /// <summary>
    /// Generate a contiguous bordered map for forest dungeon
    /// </summary>
    public void generateRandomForestMap()
    {
        perlinThreshold = .5f;
        perlinScaleFactor1 = .3f;
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
        PopulateMap();
        BakeNavMesh();
        updateColor();
        raiseWalls();
        UpdateEditorViews();
    }

    /// <summary>
    /// Raise all wall tiles.
    /// </summary>
    public void raiseWalls()
    {
        Vector3 s = tilePrefab.transform.localScale;
        foreach (GroundTile tile in tileMap)
        {
            if (tile.group != -1) continue;
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

    /// <summary>
    /// Place a player spawn circle and connect it to map if it isn't, partition map into groups, then spawn players and enemies in groups
    /// </summary>
    public void PopulateMap()
    {
        circleX = playerSpawnX;
        circleZ = playerSpawnZ;
        radius = playerSpawnRadius;
        ClearCircle(-3);
        if (FindGroups().Count > 1)
        {
            randomWalkFromSmallestGroup();
        };
        List<(int, int)> groupsAndCounts = FindGroups(makeSpawnCells: true);
        List<int> undersizedGroups = groupsAndCounts.Where(g => g.Item2 < minCellSize).Select(g => g.Item1).ToList(); // will be small and at the edges of map, good place for item spawns
        List<int> fullSizeGroups = groupsAndCounts.Where(g => g.Item2 >= minCellSize).Select(g => g.Item1).ToList(); // enemy spawn locations
        List<GroundTile> tilesInFullGroups = tileMap.Where(t => fullSizeGroups.Contains(t.group)).ToList();
        List<GroundTile> tilesInUndersizeGroups = tileMap.Where(t => undersizedGroups.Contains(t.group)).ToList();

        //// Uncomment to visualize groupings
        //foreach (GroundTile tile in tilesInUndersizeGroups)
        //{
        //    tile.group = 0;
        //}
        //foreach (GroundTile tile in tilesInFullGroups)
        //{
        //    tile.group = 1;
        //}

        List<Being> charBeings = new List<Being>(); //TODO - This was a stupid quick fix for a bug to get a recording, in case I forget to come back.
        List<Being> playerCharacters = PartyManager.instance.partyMembers.Select(p => (Being)p).ToList();
        for (int i = 0; i < LevelManager.instance.partyCount; i++)
        {
            charBeings.Add(playerCharacters[i]);
        }
        SpawnInGroupLocation(charBeings, -3);
        for(int i = 1; i < fullSizeGroups.Count; i += 5)
        {
            int group = fullSizeGroups[i];
            int count = Random.Range(1, 5);
            List<Being> beings = new List<Being>(); 
            for (int j = 0; j < count; j++)
            {
                Being being = Instantiate(enemyPrefab);
                if (being is Enemy enemy)
                {
                    enemy.group = group;
                    LevelManager.instance.enemies.Add(enemy);
                }
                beings.Add(being);
            }
            SpawnInGroupLocation(beings, group);
        }
        UpdateEditorViews();
    }
    
    /// <summary>
    /// Spawn beings at tiles with the given group number
    /// </summary>
    /// <param name="beings"></param>
    /// <param name="group"></param>
    private void SpawnInGroupLocation(List<Being> beings, int group)
    {
        List<GroundTile> spawnTiles = tileMap.Where(t => t.group == group).ToList();

        if (beings.Count > spawnTiles.Count)
        {
            Debug.LogWarning($"Not enough tiles to spawn {beings[0].name} group at tile group {group}");
        }
        Methods.Shuffle(spawnTiles);
        for (int i = 0; i < beings.Count; i++)
        {
            Vector3 p = spawnTiles[i].transform.position;
            float tileHeight = spawnTiles[i].transform.localScale.y;
            beings[i].transform.position = new Vector3(p.x, p.y + tileHeight, p.z);
            //Debug.Log($"\nbeing pos {beings[i].transform.position.x} {beings[i].transform.position.y} {beings[i].transform.position.z} \ntile pos {p.x} {p.y} {p.z}");
        }
    }

    /// <summary>
    /// Get index from x and z coords
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private int GetIndex(int x, int z)
    {
        return (x * zCount + z);
    }

    public void BakeNavMesh()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface is not assigned");
            return;
        }
        navMeshSurface.BuildNavMesh();
    }
    
}
