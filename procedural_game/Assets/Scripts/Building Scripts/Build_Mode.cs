using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Mode : MonoBehaviour
{
    //buildable objects parent object reference
    public GameObject buildable_parent_reference;

    //list of all buildable objects
    private int current_object_index = 0;
    public List<Build_Objects_Class> build_objects_list = new List<Build_Objects_Class>();

    //player reference
    public GameObject player_reference;

    //preview script references
    public Build_Preview_Script preview;


    //rotation variables
    private Quaternion object_rotation;
    private float object_y_rotation_value = 0;
    public float rotation_speed = 0f;

    //rotation key
    public KeyCode rotation_key;
    public KeyCode snap_mode_toggle_key;
    public KeyCode toggle_between_building_prefabs_key_forward;
    public KeyCode toggle_between_building_prefabs_key_backwards;

    private void Update()
    {


        if (Input.GetMouseButtonDown(1))
        {
            Build();
        }

        if (Input.GetKeyDown(rotation_key))
        {
            rotate_object();
        }

        if (Input.GetKeyDown(snap_mode_toggle_key))
        {
            toggle_snap_mode();
        }

        if (Input.GetKeyDown(toggle_between_building_prefabs_key_forward))
        {
            toggle_between_objects_forwards();
        }

        if (Input.GetKeyDown(toggle_between_building_prefabs_key_backwards))
        {
            toggle_between_objects_backwards();
        }
    }

    public void toggle_between_objects_forwards()
    {
        if(current_object_index + 1 == build_objects_list.Count)
        {
            current_object_index = 0;
        }
        else
        {
            current_object_index += 1;
        }

        preview.have_changed_object = true;

    }

    public void toggle_between_objects_backwards()
    {
        if (current_object_index == 0)
        {
            current_object_index = build_objects_list.Count - 1;
        }
        else
        {
            current_object_index -= 1;
        }

        preview.have_changed_object = true;

    }


    public void rotate_object()
    {
        object_y_rotation_value += rotation_speed;
    }


    public void Build()
    {
        GameObject built_object = (GameObject)Instantiate(get_current_object().prefab, preview.preview_output_object.transform.position, preview.preview_output_object.transform.rotation);
        built_object.transform.SetParent(buildable_parent_reference.transform);
    }

    public Build_Objects_Class get_current_object()
    {
        return build_objects_list[current_object_index];
    }


    public void destroy_preview()
    {
        Debug.Log("Destroying");
        preview.destroy_preview_object();
        preview.have_changed_object = true;
    }

    //rotation methods
    public Quaternion return_object_rotation()
    {

        object_rotation = build_objects_list[current_object_index].prefab.transform.rotation;

        //adds the y rotation to the object
        object_rotation *= Quaternion.Euler(0, object_y_rotation_value, 0);

        return object_rotation;
    }

    public void toggle_snap_mode()
    {
        if (preview.snap_mode_enabled)
        {
            preview.snap_mode_enabled = false;
        }
        else if (preview.snap_mode_enabled == false)
        {
            preview.snap_mode_enabled = true;
        }
    }





}
