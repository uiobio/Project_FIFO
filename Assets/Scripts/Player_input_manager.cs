using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_input_manager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float MoveSpeed;

    float HorizontalInput;
    float VerticalInput;
    bool DashInput;

    Vector3 MoveDirection;
    Vector3 Orientation = new Vector3(0, 0, 1);

    Rigidbody rb;

    Vector3 Up = new Vector3(-1, 0, -1);
    Vector3 Right = new Vector3(-1, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        //Disable and hide Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    
    void Update()
    {
        //Called once per frame. Reads input & updates vars
        GetInput();
        if (DashInput)
        {
            Dash(MoveDirection.normalized);
        }

        DashInput = false;
    }

    void FixedUpdate()
    {
        //Called once per set amount of time. Moves Player
        MovePlayer();
    }

    void GetInput()
    {
        //Set HorizontalInput & VerticalInput variables
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        DashInput = Input.GetButtonDown("Dash");
        
    }

    void MovePlayer()
    {
        //Moves the player rigidBody
        MoveDirection = Up * VerticalInput + Right * HorizontalInput;
        if (!MoveDirection.Equals(new Vector3(0, 0, 0))) {
            Orientation = MoveDirection.normalized;
        }
        rb.AddForce(MoveDirection.normalized * MoveSpeed, ForceMode.Force);
        

    }

    void Dash(Vector3 vect)
    {
        rb.AddForce(Orientation * 8, ForceMode.VelocityChange);
    }
}
