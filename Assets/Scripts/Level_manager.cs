using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_manager : MonoBehaviour
{
    public static Level_manager instance;

    private void Awake() //Makes levelmanager callable in any script: Level_manager.instance.[]
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Dummy")){
            Dummy();
        }
    }

    void Dummy(){
        Debug.Log("Dummy key pressed");
    }
}
