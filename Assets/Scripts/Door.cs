using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string RoomName;

    void OnTriggerEnter(Collider other)
    {
        if(FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length != 0){
            return;
        }
        GameState.Instance.roomCount++;

        // Load the Basic L Room scene when this is entered
        // FIXME: make this adjustable room type
        // Bugfix: made it so only the player can trigger the door.
        if (other.tag == "Player")
        {
<<<<<<< HEAD
<<<<<<< HEAD
            SceneManager.LoadScene(RoomName);
=======
=======
>>>>>>> baab94a4ab326b3e3fa3f23b045acd212d19d897
            Level_manager.instance.IncRoom();

            SceneManager.LoadScene(room_name);
>>>>>>> baab94a4ab326b3e3fa3f23b045acd212d19d897
        }
    }
}



