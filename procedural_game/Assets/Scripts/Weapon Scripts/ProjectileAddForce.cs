using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddForce : MonoBehaviour
{

    Rigidbody rigidb;
    public float shootForce = 2000;

    void Enable()
    {
        rigidb = GetComponent<Rigidbody>();
        rigidb.velocity = Vector3.zero;

    }

    void Update()
    {
        SpinObjectInAir();
    }

    void ApplyForce()
    {
        rigidb.AddRelativeForce(Vector3.forward * shootForce);
    }

    void SpinObjectInAir()
    {
        //spin arrow
    }


}
