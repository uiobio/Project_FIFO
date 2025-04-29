using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    // Seed for the random number generator
    public int Seed = 0;
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
        RoomMap.Add(startCoords, RootRoom);
        RoomGenPath.Push(startCoords);

        RoomWidth = RootFloorCollider.GetComponent<Renderer>().bounds.extents.x * 2;
    }

    void GenerateLevel() {
        GenerateFloors();
    }

    void GenerateFloors() {
        while (RoomMap.Count < NumRooms) {
            if (RoomGenPath.Count == 0) {
                Debug.LogWarning("Room generation failed: couldn't reach target room count.");
                return;
            }
            Direction nextDirection = Direction.NE; // Default direction
            List<Direction> availableDirections = new();
            Vector2Int currentCoords = RoomGenPath.Peek();
            Vector2Int NECoords = currentCoords + DirectionMap[Direction.NE];
            Vector2Int NWCoords = currentCoords + DirectionMap[Direction.NW];
            Vector2Int SECoords = currentCoords + DirectionMap[Direction.SE];
            Vector2Int SWCoords = currentCoords + DirectionMap[Direction.SW];
            if (!RoomGenVisited.Contains(NECoords)) availableDirections.Add(Direction.NE);
            if (!RoomGenVisited.Contains(NWCoords)) availableDirections.Add(Direction.NW);
            if (!RoomGenVisited.Contains(SECoords)) availableDirections.Add(Direction.SE);
            if (!RoomGenVisited.Contains(SWCoords)) availableDirections.Add(Direction.SW);
            if (availableDirections.Count == 0)
            {
                // No available directions, so we need to backtrack
                RoomGenPath.Pop();
                continue;
            }
            else {
                nextDirection = availableDirections[rng.Next(availableDirections.Count)];
            }
            Vector2Int nextCoords = currentCoords + DirectionMap[nextDirection];
            InitializeNewRoom(nextCoords);
            
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
