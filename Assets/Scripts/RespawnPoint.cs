using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject spawneePrefab;
    private GameObject spawnee;
    [SerializeField]
    private bool spawnsEnemy;
    [SerializeField]
    private int enemyElement;
    [SerializeField]
    private GameObject pWarning;
    private GameObject iWarning;
    private bool spawning;

    // Start is called before the first frame update
    void Start()
    {
        if (spawneePrefab != null)
        {
            StartSpawn();
        }
    }

    void Awake()
    {
        spawning = false;
    }

    public void StartSpawn()
    {
        if (spawning == true)
        {
            Debug.Log($"{gameObject.name} tried and failed to spawn");
            return;
        }
        Debug.Log(spawnsEnemy);
        Debug.Log($"Setting spawning to TRUE for {gameObject.name}");
        spawning = true;
        if (!spawnsEnemy)
        {
            Spawn();
            return;
        }
        iWarning = Instantiate(pWarning, transform);
        Invoke("Spawn", LevelManager.Instance.EnemySpawnWarningTime);
    }

    void Spawn()
    {
        if (iWarning != null) 
        {
            //Destroy the warning visuals/object
            Destroy(iWarning);
        }
        if (spawneePrefab != null) 
        {
            spawnee = Instantiate(spawneePrefab, transform.position, Quaternion.identity);
            //Set element if enemy
            if (spawnsEnemy)
            {
                spawnee.GetComponent<Enemy>().SetElement(enemyElement);
            }
            Debug.Log($"Spawned {spawnee.gameObject.name}. Set elem to {enemyElement}? {spawnsEnemy}");
        }
        spawning = false;
    }

    public void SetSpawnee(GameObject newSpawnee)
    {
        SetSpawnee(newSpawnee, false, false, -1);
    }
    public void SetSpawnee(GameObject newSpawnee, bool spawn, bool isEnemy, int element)
    {
        Debug.Log("Setting Spawnee");
        spawneePrefab = newSpawnee;
        spawnsEnemy = isEnemy;
        if (spawnsEnemy)
        {
            enemyElement = element;
        }
        if (spawn) 
        { 
            StartSpawn();
        }
    }
}
