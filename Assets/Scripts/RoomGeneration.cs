using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
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
    // Rooms adjacent to the Root Room: [NE, NW, SE, SW] have coordinates [(1,0), (0,1), (-1,0), (0,-1)] respectively
    // Other rooms will follow this same pattern
    // i.e.: NE = +X
    //       SE = -X
    //       NW = +Z
    //       SW = -Z
    [System.NonSerialized]
    public Dictionary<Vector2Int, GameObject> RoomMap = new();

    [System.NonSerialized]
    public float RoomWidth = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        // GameObject.FindWithTag("MainCamera").GetComponent<Camera_manager>().Player = SpawnPoint.GetComponent<Respawn_Point>().Spawnee;
        Destroy(SpawnPoint);
        // Debug.Log(GameObject.FindWithTag("MainCamera").GetComponent<Camera_manager>().Player.name);
        RoomMap.Add(new Vector2Int(0, 0), RootRoom);

        RoomWidth = RootFloorCollider.GetComponent<Renderer>().bounds.extents.x * 2;
    }

    void GenerateLevel() {
        // PSEUDOCODE:
        // set a variable "currentRoom" to the Root Room -- (0, 0) in RoomMap
        // for i from 1 to NumRooms, 1 because we've already generated the root room
        //     Randomly select a direction from the currentRoom
        //     If theres already a room in that direction
        //         Re-select a direction
        //     If there's not a room in that direction
        //         Generate a new room in that direction
        //     Add the new room to RoomMap with the coordinates of the new room.
        //     Set the currentRoom to the new room.
        // put the exit Door in the furthest room from the Root Room.
    }

    void AttemptRoom(Direction direction)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

enum Direction
{
    NE,
    NW,
    SE,
    SW
}
