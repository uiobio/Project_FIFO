using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Point : MonoBehaviour
{
    [SerializeField]
    GameObject Spawnee;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Instantiate(Spawnee, transform.position, Quaternion.identity);
    }
}
