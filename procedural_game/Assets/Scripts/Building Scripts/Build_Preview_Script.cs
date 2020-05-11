using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Preview_Script : MonoBehaviour
{
    public Build_Mode build_mode_reference;

    private Build_Objects_Class current_object;

    public Material preview_material;

    public float build_distance = 10f;

    //the little cube attatched to player that says where to build
    public Transform build_position_reference;

    public Camera player_camera;

  


    //snap variables
    public float grid_size = 1f;

    //snap mode free mode toggle
    public bool snap_mode_enabled = true;

    //private variables

    //preview object reference
    [HideInInspector]
    public GameObject preview_output_object;

    //has the preview object changed?
    [HideInInspector]
    public bool have_changed_object = true;

    //check if theres already a preview at that position 
    private RaycastHit raycast_end;
    LayerMask preview_layer;

    private Transform build_position;


    //scroll_speed_float
    public float scroll_speed;
    private float scroll_distance = 7;
    public int min_scroll_build_distance;
    public int max_scroll_build_distance;


    void get_current_object()
    {
        current_object = build_mode_reference.get_current_object();
    }


    private Quaternion get_current_rotation()
    {
        Quaternion object_rotation = build_mode_reference.return_object_rotation();

        return object_rotation;
    }

    void get_preview_position()
    {

        build_position = build_position_reference.transform;

        build_position.Rotate(get_current_rotation().eulerAngles);

    }


  
    




    void create_preview_object()
    {

        get_preview_position();
        //create new preview_object reference
        GameObject preview_object = current_object.prefab;
   
        //instantiate the object at the build_position

        preview_output_object = (GameObject)Instantiate(preview_object, build_position.position, build_position.rotation);
        preview_output_object.GetComponent<Renderer>().material = preview_material;
        preview_output_object.GetComponent<Collider>().enabled = false;
        have_changed_object = false;

    }

    void update_preview_position()
    {

        if (snap_mode_enabled)
        {
            Vector3 rounded = new Vector3(Mathf.Round(build_position.position.x / grid_size) * grid_size, Mathf.Round(build_position.position.y / grid_size) * grid_size, Mathf.Round(build_position.position.z / grid_size) * grid_size);
            preview_output_object.transform.position = rounded;
            
        }

        else
        {
            preview_output_object.transform.position = build_position.position;
        }

        preview_output_object.transform.rotation = build_mode_reference.return_object_rotation();

    }

    public static MCFace detect_face_hit()
    {
        return MCFace.East;
    }

    public void destroy_preview_object()
    {
        Destroy(preview_output_object);

    }


    void scroll_method()
    {

        var scrooling = Input.GetAxis("Mouse ScrollWheel");


        
        if (scrooling > 0f && scroll_distance < max_scroll_build_distance)
        {
            scroll_distance += 1 * scroll_speed;

            build_position_reference.transform.localPosition = new Vector3(0, 0.07f, scroll_distance);

        }

        if (scrooling < 0f && scroll_distance > min_scroll_build_distance)
        {
            scroll_distance -= 1 * scroll_speed;

            build_position_reference.transform.localPosition = new Vector3(0, 0.07f, scroll_distance);
        }
        

        


    }




    void Update()
    {
        

        get_current_object();

        if (have_changed_object)
        {
            destroy_preview_object();
            create_preview_object();
            have_changed_object = false;
        }

        scroll_method();
        update_preview_position();
       
    }







}
