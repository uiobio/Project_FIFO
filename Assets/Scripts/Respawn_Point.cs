using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Point : MonoBehaviour
{
    [SerializeField]
    private GameObject SpawneePrefab;
    private GameObject spawnee;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        spawnee = Instantiate(SpawneePrefab, transform.position, Quaternion.identity);
    }
}
