using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinInAir : MonoBehaviour
{

    void Update()
    {
        SpinObjectInAir();
    }
    void SpinObjectInAir()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float yVelocity = rb.velocity.y;
        float zVelocity = rb.velocity.z;
        float xVelocity = rb.velocity.x;

        float combinedVelocity = Mathf.Sqrt((xVelocity * xVelocity) + (zVelocity * zVelocity));

        float fallAngle =  Mathf.Atan2(yVelocity, combinedVelocity) * (180 / Mathf.PI);

        transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);




    }
}
