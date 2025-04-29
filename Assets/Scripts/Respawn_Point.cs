using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Point : MonoBehaviour
{
    [SerializeField]
    private GameObject SpawneePrefab;
    private GameObject spawnee;
    [SerializeField]
    private bool SpawnsEnemy;
    [SerializeField]
    private int EnemyElement;
    [SerializeField]
    private GameObject p_Warning;
    private GameObject I_Warning;
    private bool spawning;

    // Start is called before the first frame update
    void Start()
    {
        spawning = false;
        if(SpawneePrefab != null){
            StartSpawn();
        }
    }

    public void StartSpawn(){
        spawning = true;
        if(!SpawnsEnemy){
            Spawn();
            return;
        }
        I_Warning = Instantiate(p_Warning, transform);
        Invoke("Spawn", Level_manager.instance.EnemySpawnWarningTime);
    }

    void Spawn()
    {
        if(I_Warning != null){
            //Destroy the warning visuals/object
            Destroy(I_Warning);
        }
        if(SpawneePrefab != null){
            spawnee = Instantiate(SpawneePrefab, transform.position, Quaternion.identity);
            //Set element if enemy
            if(SpawnsEnemy){
                spawnee.GetComponent<Enemy>().SetElement(EnemyElement);
            }
            Debug.Log($"Spawned {spawnee.gameObject.name}. Set elem to {EnemyElement}? {SpawnsEnemy}");
        }
        spawning = false;
    }

    public void SetSpawnee(GameObject new_spawnee){
        SetSpawnee(new_spawnee, false, false, -1);
    }
    public void SetSpawnee(GameObject new_spawnee, bool spawn, bool isEnemy, int Element){
        SpawneePrefab = new_spawnee;
        SpawnsEnemy = isEnemy;
        if(SpawnsEnemy){
            EnemyElement = Element;
        }
        if (spawn) { StartSpawn(); }
    }
}
