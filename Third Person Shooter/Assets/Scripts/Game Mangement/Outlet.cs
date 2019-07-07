using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlet : MonoBehaviour
{
    /*Desc: Class which defines all variables and functions of an outlet. 
     *All game objects functioning as an outlet must have this script atteched.
    */

    //Defines the order in which outlet must be plugged into by player
    public int PlugOrder;

    //Bool set to true if light is plugged into an outlet
    private bool isPluggedInto;
    public bool IsPluggedInto
    {
        get
        {
            return isPluggedInto;
        }
        private set { isPluggedInto = value; }
    }

    private GameObject pluggedInLight;

    // Start is called before the first frame update
    void Start()
    {
        //Lights start not plugged into
        pluggedInLight = null;
        IsPluggedInto = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function that updates object to reflect a plugged in state.
    public void PlugInLight(GameObject light)
    {
        pluggedInLight = light;
        IsPluggedInto = true; 
    }

    //Function that updates object to reflect an unplugged in state.
    public void UnPlugLight()
    {
        //Unplug light if light object is associated
        if (pluggedInLight != null)
        {
            PluggableLight light = pluggedInLight.GetComponent<PluggableLight>();
            light.UnPlugLight();
        }

        //Set Properties and private vars to refelct unplugged state
        pluggedInLight = null;
        IsPluggedInto = false;

    }
}
