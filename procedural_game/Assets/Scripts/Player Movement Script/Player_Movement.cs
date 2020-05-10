using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //reference to controller
    public CharacterController controller;

    //speed the player moves at 
    public float speed = 12f;

    //gravity constant
    public float gravity = -9.81f;
    //for gravity
    Vector3 velocity;

    //jump height
    public float jumpHeight = 3f;

    //reference to groundCheck object
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    //the layer mask will determine what counts as the ground
    public LayerMask groundMask;
    public bool isGrounded;
    public LayerMask buildMask;
    public bool isGroundedBuildMask;



    // Update is called once per frame
    void Update()
    {

        //we on the ground?
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGroundedBuildMask = Physics.CheckSphere(groundCheck.position, groundDistance, buildMask);


        if (isGrounded && velocity.y < 0 || isGroundedBuildMask && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gets input from wasd keys (automatically maps to controlleres too)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        //consider it an arrow in the direction that we want to move, we want to make it local coordinates
        //not global coordinates, so we want to take the direction the player is facing into account
        //the transform.right and transform.forward tell us which way the player is facing
        Vector3 move = transform.right * x + transform.forward * z;


        //actual movement
        controller.Move(move * speed * Time.deltaTime);

        //jump script
        if (Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Jump") && isGroundedBuildMask)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);



    }
}
