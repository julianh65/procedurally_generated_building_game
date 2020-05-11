using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    public GameObject build_manager;

    //keys to activate things

    public KeyCode build_mode_key;



        void Update()
    {


        //toggle BuildMode


        if (Input.GetKeyDown(build_mode_key))
        {

            if (build_manager.GetComponent<Build_Mode>().enabled == false)
            {
                build_manager.GetComponent<Build_Mode>().enabled = true;
                build_manager.GetComponent<Build_Preview_Script>().enabled = true;

            }

            else if (build_manager.GetComponent<Build_Mode>().enabled == true)
            {
                build_manager.GetComponent<Build_Preview_Script>().enabled = false;
                build_manager.GetComponent<Build_Mode>().destroy_preview();
                build_manager.GetComponent<Build_Mode>().enabled = false;

            }



        

        







        }

    }
}
