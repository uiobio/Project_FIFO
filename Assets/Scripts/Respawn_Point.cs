using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Point : MonoBehaviour
{
    [SerializeField]
    GameObject Spawnee;
    [SerializeField]
    GameObject ShopItem;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        Camera_manager.instance.Player = Spawnee;
    }

    void Spawn()
    {
        Spawnee = Instantiate(Spawnee, transform.position, Quaternion.identity);
        Instantiate(ShopItem, new Vector3(5.85f, 0.4f, 0.6f), Quaternion.identity);
    }
}
