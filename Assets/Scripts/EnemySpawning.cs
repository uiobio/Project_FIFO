using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EnemySpawning : MonoBehaviour
{    
    public List<GameObject> EnemyRanks = new List<GameObject>();

    public List<int> Amounts = new List<int>();

    public List<GameObject> allSpawners = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Respawn_Point[] allObjects = (Respawn_Point[])FindObjectsByType<Respawn_Point>(FindObjectsSortMode.None);
        foreach (Respawn_Point RP in allObjects)
        {
            allSpawners.Add(RP.gameObject);
        }
        SpawnEnemies();
    }


    void SpawnEnemies(){
        List<int> _rank = new List<int>(); //Holds indexes of ranks
        //Setup the list of how many of each rank is needed
        for(int r=0; r < EnemyRanks.Count; r++){
            _rank.AddRange(Enumerable.Repeat(r, Amounts[r]));
        }

        if (Amounts.Count != EnemyRanks.Count){
            Debug.LogWarning("Mismatch in number of Enemy Ranks and number of provided Enemy Amounts!");
        }
        else{
            for(int i=0; i < _rank.Count; i++){
                //Select random position for enemy of rank i
                int p_rand = (int)Random.Range(0f, (float)allSpawners.Count-1);
                int r = _rank[i];
                int element = GetElement();
                //Spawn enemy of rank r at position p_rand
                allSpawners[p_rand].GetComponent<Respawn_Point>().SetSpawnee(EnemyRanks[r], true, true, element);
                Debug.Log($"Set {EnemyRanks[r].name} to spawn at position {p_rand}");
                allSpawners.RemoveAt(p_rand);
            }
        }
    }

    int GetElement(){
        // Returns an integer representing the element of the enemy.
        // This is a separate function to allow for more robust element spawning logic in the future.
        return (int)Random.Range(0f, 3f);
    }
}
