using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject stringReference;

    public GameObject arrowPrefab;

    public Transform arrow_position;

    public GameObject bow;

    private GameObject currentArrow;

    private int shootForce = 10;

    public Camera forwardShootDirection;

    public int drawSpeed;



    public int numberOfArrows = 10;


    bool arrowSlotted = false;


    float pullbackDistance = -9.6f;
    float pullSpeed = 0.3f;

    public float maxPullbackDistance;


    void Update()
    {

        SpawnArrow();

        currentArrow.transform.position = arrow_position.transform.position;

        currentArrow.transform.eulerAngles = arrow_position.transform.eulerAngles;




        if (Input.GetMouseButton(1))
        {
            pullBack();
            
        }

        if (Input.GetMouseButtonUp(1))
        {
            release();


        }

    }


    void SpawnArrow()
    {
        if (numberOfArrows > 0 && arrowSlotted == false)
        {
            arrowSlotted = true;
            currentArrow = Instantiate(arrowPrefab, arrow_position.position, arrow_position.rotation) as GameObject;
            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }


    void pullBack()
    {
        if (numberOfArrows > 0)
        {
            if (pullbackDistance > maxPullbackDistance)
            {
                pullbackDistance -= 1 * pullSpeed;
                stringReference.transform.localPosition = new Vector3(0, pullbackDistance, 0);
            }


        }

        if(shootForce < 100)
        {
            shootForce += drawSpeed;

        }


    }

    void release()
    {

        arrowSlotted = false;
        pullbackDistance = -9.6f;
        stringReference.transform.localPosition = new Vector3(0, pullbackDistance, 0);


        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;



        rb.AddForce(forwardShootDirection.transform.forward.normalized * shootForce, ForceMode.Impulse);


        currentArrow.GetComponent<despawn>().enabled = true;
        numberOfArrows -= 1;
        shootForce = 10;


    }



}

