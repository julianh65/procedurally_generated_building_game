using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject stringReference;

    public GameObject arrowPrefab;

    public GameObject arrow;

    public GameObject bow;



    public int numberOfArrows = 10;


    bool arrowSlotted = false;

    float pullbackDistance = -9.6f;
    float pullSpeed = 0.3f;
    public float maxPullbackDistance;

    void Start()
    {
        SpawnArrow();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pullBack();
        }
    }


    void SpawnArrow()
    {
        if (numberOfArrows > 0)
        {
            arrowSlotted = true;
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
            arrow.transform.parent = transform;
        }
    }


    void pullBack()
    {
        if (numberOfArrows > 0)
        {
            if(pullbackDistance > maxPullbackDistance)
            {
                pullbackDistance -= 1 * pullSpeed;
                stringReference.transform.localPosition = new Vector3(0, pullbackDistance, 0);
            }
        }
           
    }

}

