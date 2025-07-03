using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        GameObject p = GameObject.FindWithTag("Player");

        if (p != null)
        {
            player = p;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(
            player.transform.position.x + offset.x,
            player.transform.position.y + offset.y,
            player.transform.position.z + offset.z
        );
    }
}
