using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Load the Basic L Room scene when this is entered
        //FIXME: make this adjustable room type
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("Basic L Room");
        }
    }
}



