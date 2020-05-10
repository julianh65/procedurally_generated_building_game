using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mouse_Look : MonoBehaviour
{
    //mouse sensetivity
    public float mouseSensitivity = 100f;

    //reference to the playerbody
    public Transform playerBody;

    //rotation variable
    float xRotation = 0f;

    void Start()
    {
        //code that hides and locks cursor to center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //get mouse x
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //get mouse y
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        //apply the rotation around the object (up down)
        xRotation -= mouseY;
        //clamp so we can't look behind player
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //apply the x rotation (left right)
        playerBody.Rotate(Vector3.up * mouseX);


    }
}
