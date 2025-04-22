using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public GameObject FloorPrefab;
    [System.NonSerialized]
    public GameObject RootFloor;
    [System.NonSerialized]
    public GameObject RootFloorCollider;
    [System.NonSerialized]
    public GameObject RootFloorSprite;
    [System.NonSerialized]
    public GameObject RootFloorSpawners;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RootFloor = Instantiate(FloorPrefab);
        RootFloor.name = "RootFloor";
        RootFloorCollider = RootFloor.transform.Find("Collider").gameObject;
        RootFloorSprite = RootFloor.transform.Find("Sprite").gameObject;
        Debug.Log("Root Floor X Bound: " + RootFloorCollider.GetComponent<Renderer>().bounds.extents.x);
        Debug.Log("Root Floor Z Bound: " + RootFloorCollider.GetComponent<Renderer>().bounds.extents.z);
        GameObject Floor = Instantiate(FloorPrefab);
        Floor.name = "Floor";
        Floor.transform.position = new Vector3(RootFloor.transform.position.x, 0, RootFloor.transform.position.z) + new Vector3(RootFloorCollider.GetComponent<Renderer>().bounds.extents.x * 2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
