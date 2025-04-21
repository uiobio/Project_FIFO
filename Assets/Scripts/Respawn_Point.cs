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

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if(SpawneePrefab != null){
            spawnee = Instantiate(SpawneePrefab, transform.position, Quaternion.identity);
            //Set element if enemy
            if(SpawnsEnemy){
                spawnee.GetComponent<Enemy>().SetElement(EnemyElement);
            }
            Debug.Log($"Spawned {spawnee.gameObject.name}. Set elem to {EnemyElement}? {SpawnsEnemy}");
        }
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
        if (spawn) { Spawn(); }
    }
}
