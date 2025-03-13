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

    // Most upgrades the player can have at once
    public const int MAX_PLAYER_UPGRADES = 5;

    // The longest an element pattern can be
    public const int MAX_PATTERN_LEN = 3;

    // TO ACCESS: use GameState.Instance.[variable name]
    // NOTE: this must be Instance with captial I
    public int roomCount;
    public int Currency;
    public float PlayerHealth;
    public float PlayerMaxHealth;
    public int[] PlayerHeldUpgradeIds = new int[MAX_PLAYER_UPGRADES];

    // Use this for intiialization logic instead of Start(), because Start() may be called
    // too late for other scripts to see data in this object.
    private static void Initialize()
    {
        instance = new GameObject().AddComponent<GameState>();
        instance.name = "Game State";

        instance.roomCount = 0;

        instance.PlayerMaxHealth = 10;

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
