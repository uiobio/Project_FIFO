using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    string RoomName;

    void OnTriggerEnter(Collider other)
    {
        if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length != 0)
        {
            return;
        }
        GameState.Instance.roomCount++;

        // Load the Basic L Room scene when this is entered
        // FIXME: make this adjustable room type
        // Bugfix: made it so only the player can trigger the door.
        if (other.tag == "Player")
        {
            LevelManager.Instance.IncRoom();

            SceneManager.LoadScene(RoomName);
        }
    }
}



