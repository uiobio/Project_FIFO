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

    public void Spawn()
    {
        if(SpawneePrefab != null){
            spawnee = Instantiate(SpawneePrefab, transform.position, Quaternion.identity);
            Debug.Log($"Spawned {spawnee.gameObject.name}");
        }
    }

    public void SetSpawnee(GameObject new_spawnee){
        SetSpawnee(new_spawnee, false);
    }
    public void SetSpawnee(GameObject new_spawnee, bool spawn){
        SpawneePrefab = new_spawnee;
        if (spawn) { Spawn(); }
    }
}
