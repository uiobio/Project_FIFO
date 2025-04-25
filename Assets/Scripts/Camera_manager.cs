using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_manager : MonoBehaviour
{
    public static Camera_manager instance;
    [Header("Camera")]
    public GameObject Player;
    [SerializeField]
    private float[] cameraOffsetXYZ = new float[3];

    private void Awake() // Camera_manager callable in any script: Camera_manager.instance.[]
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; 
        if (Player == null)
        {
            // Debug.Log("Camera_manager: Player is null");
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                // Debug.Log("Camera_manager Start() called. Player name: " + p.name);
                Player = p;
            }
        }
        else
        {
            // Debug.Log("Camera_manager Player is not null. Player name: " + Player.name);
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x + cameraOffsetXYZ[0], Player.transform.position.y + cameraOffsetXYZ[1], Player.transform.position.z + cameraOffsetXYZ[2]);
    }
}
