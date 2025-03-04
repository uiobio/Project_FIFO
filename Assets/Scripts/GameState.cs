using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton holding all the game constants
public class GameState : MonoBehaviour
{
    private static GameState instance;

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                Initialize();
            }

            return instance;
        }
    }

    public int roomCount;

    // Use this for intiialization logic instead of Start(), because Start() may be called
    // too late for other scripts to see data in this object.
    private static void Initialize()
    {
        instance = new GameObject().AddComponent<GameState>();
        instance.name = "Game State";

        instance.roomCount = 0;

        DontDestroyOnLoad(instance.gameObject);

        Debug.Log("Game State Singleton Initialized");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        // if (Instance != null && Instance != this)
        // {
        //     // if there's an instance other than this, destroy it
        //     Destroy(this);
        // }
        // else
        // {
        //     DontDestroyOnLoad(this);
        // }
    }
}
