using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    string room_name;

    void OnTriggerEnter(Collider other)
    {
        // Load the Basic L Room scene when this is entered
        // FIXME: make this adjustable room type
        // Bugfix: made it so only the player can trigger the door.
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(room_name);

            GameState.Instance.roomCount++;

            // Git restore upgrade - restore x% of health when leaving a room
            float playerMaxHealth = Level_manager.instance.PlayerMaxHealth;
            float restorePercent = 0.01f * Level_manager.instance.gitRestoreUpgradeModifier;
            float restoreAmount = playerMaxHealth * restorePercent;

            float newHealth = Level_manager.instance.PlayerHealth + restoreAmount;
            Level_manager.instance.SetHealth(Math.Min(playerMaxHealth, newHealth));

            Debug.Log("Player entered a door");
        }
    }
}



