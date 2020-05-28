using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawn : MonoBehaviour
{
           

    public float TimeToLive = 5f;

    void Start()
    {

        Destroy(gameObject, TimeToLive);
    }




}
