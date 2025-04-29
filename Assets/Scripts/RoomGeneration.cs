using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    // If true, the seed for the random number generator will be randomized
    public bool RandomSeed = false;

    // Seed for the random number generator
    public int Seed = 0;

    // Weight for continuing to generate rooms in the same direction (higher = longer corridors, 1 is no extra weight)
    public int SameDirectionWeight = 1;
    private System.Random rng;

    // Number of rooms to generate
    public int NumRooms = 5;

    // Prefabs
    public GameObject RespawnPrefab;
    public GameObject FloorPrefab;

    // Root Room Stuff
    [System.NonSerialized]
    public GameObject RootRoom;
    [System.NonSerialized]
    public GameObject RootFloor;
    [System.NonSerialized]
    public GameObject SpawnPoint;
    [System.NonSerialized]
    public GameObject RootFloorCollider;
    [System.NonSerialized]
    public GameObject RootFloorSprite;
    [System.NonSerialized]
    public GameObject RootFloorSpawners;

    // Parent GameObject of all Rooms in the Level
    // Note that this GameObject is the one that the script is attached to
    // It's just an explicit field here for clarity
    [System.NonSerialized]
    public GameObject Level;

    // Dictionary of coordinates to Room GameObjects;
    // (0,0) is the Root Room
    // Rooms adjacent to the Root Room: [NE, NW, SE, SW] have coordinates [(-1,0), (0,-1), (0,1), (1,0)] respectively
    // Other rooms will follow this same pattern
    // i.e.: NE = -X
    //       SE = +Z
    //       NW = -Z
    //       SW = +X
    [System.NonSerialized]
    public Dictionary<Vector2Int, GameObject> RoomMap = new();

    [System.NonSerialized]
    public Dictionary<Direction, Vector2Int> DirectionMap = new()
    {
        { Direction.NE, new Vector2Int(-1, 0) },
        { Direction.NW, new Vector2Int(0, -1) },
        { Direction.SE, new Vector2Int(1, 0) },
        { Direction.SW, new Vector2Int(0, 1) }
    };

    [System.NonSerialized]
    public Stack<Vector2Int> RoomGenPath = new();

    [System.NonSerialized]
    public HashSet<Vector2Int> RoomGenVisited = new();

    [System.NonSerialized]
    public float RoomWidth = 0;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RandomSeed) {
            Seed = (int)DateTime.Now.Ticks;
        }

        rng = new System.Random(Seed);
        Level = gameObject;
        InitializeRootRoom();
        GenerateLevel();
    }

    void InitializeRootRoom() {
        RootFloor = Instantiate(FloorPrefab);
        SpawnPoint = Instantiate(RespawnPrefab);
        SpawnPoint.GetComponent<Respawn_Point>().Spawn();

        RootFloor.name = "RootFloor";
        RootFloor.transform.position = new Vector3(0, 0, 0);
        RootFloorCollider = RootFloor.transform.Find("Collider").gameObject;
        RootFloorSprite = RootFloor.transform.Find("Sprite").gameObject;
        RootFloorSpawners = RootFloor.transform.Find("Spawners").gameObject;
        Destroy(RootFloorSpawners); // We don't want enemies in the room the player spawns in, to avoid the player immediately getting swarmed
        
        RootRoom = new GameObject("RootRoom");
        RootRoom.transform.position = new Vector3(0, 0, 0);
        RootRoom.transform.SetParent(Level.transform);
        RootFloor.transform.SetParent(RootRoom.transform);

        SpawnPoint.transform.position = new Vector3(0, 0, 0);
        SpawnPoint.transform.SetParent(RootRoom.transform);
        Destroy(SpawnPoint);

        Vector2Int startCoords = new(0, 0);
        RoomGenVisited.Add(startCoords);
        RoomGenPath.Push(startCoords);
        RoomMap.Add(startCoords, RootRoom);

        // We we don't want to generate rooms adjacent to the Root Room, so that the player has a clear initial direction to travel.
        RoomGenVisited.Add(new Vector2Int(1, 1));
        RoomGenVisited.Add(new Vector2Int(1, -1));
        RoomGenVisited.Add(new Vector2Int(-1, 1));
        RoomGenVisited.Add(new Vector2Int(-1, -1));
        Direction initDirection = DirectionMap.Keys.ElementAt(rng.Next(DirectionMap.Count));
        foreach (Direction dir in DirectionMap.Keys)
        {
            if (dir == initDirection) continue;
            Vector2Int coords = startCoords + DirectionMap[dir];
            RoomGenVisited.Add(coords);
        }

        RoomWidth = RootFloorCollider.GetComponent<Renderer>().bounds.extents.x * 2;
    }

    void GenerateLevel() {
        GenerateFloors();
    }

    void GenerateFloors() {
        GenerationMode currentMode = GenerationMode.Hallway;
        int roomsSinceLastSwitch = 0;
        const int SwitchThreshold = 3;
        Direction lastDirection = Direction.NE; // Default direction
        while (RoomMap.Count < NumRooms) {
            if (RoomGenPath.Count == 0) {
                Debug.LogWarning("Room generation failed: couldn't reach target room count.");
                return;
            }
            if (roomsSinceLastSwitch >= SwitchThreshold) {
                if (rng.NextDouble() < 0.5) {
                    currentMode = (currentMode == GenerationMode.Hallway) ? GenerationMode.BigRoom : GenerationMode.Hallway;
                }
                roomsSinceLastSwitch = 0;
            }
            if (currentMode == GenerationMode.Hallway) {
                GenerateSingleRoom(ref lastDirection);
                roomsSinceLastSwitch++;
            }
            else if (currentMode == GenerationMode.BigRoom) {
                if (GenerateBigRoom()) {
                    roomsSinceLastSwitch++;
                }
                else {
                    // If failed to place a big room (no space), fallback to hallway
                    currentMode = GenerationMode.Hallway;
                }
            }
        }
    }


    void InitializeNewRoom(Vector2Int coords) {
        GameObject newFloor = Instantiate(FloorPrefab);
        newFloor.name = "Floor";
        newFloor.transform.position = new Vector3(coords.x * RoomWidth, 0, coords.y * RoomWidth);

        GameObject newRoom = new GameObject($"Room ({coords.x}, {coords.y})");
        newRoom.transform.position = new Vector3(coords.x * RoomWidth, 0, coords.y * RoomWidth);

        newRoom.transform.SetParent(Level.transform);
        newFloor.transform.SetParent(newRoom.transform);
        
        RoomGenVisited.Add(coords);
        RoomGenPath.Push(coords);
        RoomMap.Add(coords, newRoom);
    }

    void GenerateSingleRoom(ref Direction lastDirection) {
        if (RoomGenPath.Count == 0)
        {
            Debug.LogWarning("Room generation failed: couldn't reach target room count.");
            return;
        }
        Direction nextDirection = Direction.NE; // Default direction
        List<Direction> availableDirections = new();
        Vector2Int currentCoords = RoomGenPath.Peek();
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            Vector2Int neighborCoords = currentCoords + DirectionMap[dir];
            if (!RoomGenVisited.Contains(neighborCoords))
            {
                availableDirections.Add(dir);
            }
        }
        if (availableDirections.Count == 0)
        {
            // No available directions, so we need to backtrack
            RoomGenPath.Pop();
            return;
        }

        List<Direction> weightedDirections = new();
        foreach (Direction dir in availableDirections)
        {
            if (dir == lastDirection)
            {
                // Favor continuing forward
                for (int i = 0; i < SameDirectionWeight; i++) weightedDirections.Add(dir);
            }
            else
            {
                weightedDirections.Add(dir); // Normal chance
            }
        }

        nextDirection = weightedDirections[rng.Next(weightedDirections.Count)];
        Vector2Int nextCoords = currentCoords + DirectionMap[nextDirection];
        InitializeNewRoom(nextCoords);
        lastDirection = nextDirection;
    }

    bool GenerateBigRoom()
    {
        Vector2Int currentCoords = RoomGenPath.Peek();
        List<Direction> availableDirections = new();

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            Vector2Int neighborCoords = currentCoords + DirectionMap[dir];
            if (!RoomGenVisited.Contains(neighborCoords))
            {
                availableDirections.Add(dir);
            }
        }

        if (availableDirections.Count == 0)
        {
            RoomGenPath.Pop();
            return false;
        }

        Direction baseDirection = availableDirections[rng.Next(availableDirections.Count)];
        Vector2Int baseCoords = currentCoords + DirectionMap[baseDirection];

        // Randomly pick big room size (2x2, 2x3, etc.)
        int width = rng.Next(2, 4);  // 2 to 3
        int height = rng.Next(2, 4); // 2 to 3

        // Collect all coords we would occupy
        List<Vector2Int> newCoords = new();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int offset = new Vector2Int(x, y);
                Vector2Int coord = baseCoords + offset;
                if (RoomGenVisited.Contains(coord))
                {
                    return false; // Abort placing this big room
                }
                newCoords.Add(coord);
            }
        }

        // Actually place the big room tiles
        foreach (var coord in newCoords)
        {
            InitializeNewRoom(coord);
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Direction
{
    NE,
    NW,
    SE,
    SW
}

public enum GenerationMode
{
    Hallway,
    BigRoom
}
