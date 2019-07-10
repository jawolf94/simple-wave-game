using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluggableLight : MonoBehaviour
{
    /*Desc: Class which defines all variables and functions of a light. 
     *All game objects functioning as a light must have this script atteched.
    */

    //Materials set in Unity UI determining the light's appearence when on/off
    public Material lightOn;
    public Material lightOff;

    //Properties

    //True if light is in range of an outlet
    private bool outletInRange;
    public bool OutletInRange {
        get { return outletInRange; }
        private set { outletInRange = value;}
    }

    //True if light is plugged in to an outlet 
    private bool pluggedIn;
    public bool PluggedIn
    {
        get
        {
            return pluggedIn;
        }

        private set
        {
            pluggedIn = value;
        }

    }

    //Instance of GameManager
    private GameManager GameManager;

    //Renderer to update material
    private Renderer lightRenderer;
    
    //GameObject of nearby outlet. Null if not in range 
    private GameObject nearbyOutlet;

    // Start is called before the first frame update
    void Start()
    {
        //Find scene's instance of Game Manager
        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        //Get attached Renderer component. Lights are initially off
        lightRenderer = GetComponent<Renderer>();
        lightRenderer.material = lightOff;
        
        OutletInRange = false;
        nearbyOutlet = null;
        
    }


    public void OnTriggerEnter(Collider other)
    {
        //Set light in range if it enters outlet trigger
        if (other.tag.Equals("Outlet"))
        {
            OutletInRange = true;
            nearbyOutlet = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Set light out of ranfe if it exits outlet trigger
        if (other.tag.Equals("Outlet"))
        {
            OutletInRange = false;
            nearbyOutlet = null;
        }

    }

    //Sets Light object to a plugged in state
    public void PlugInLight()
    {
        //Light is flagged as plugged in
        PluggedIn = true;

        //Material set to on material 
        lightRenderer.material = lightOn;

        //Update Outlet object
        Outlet o = nearbyOutlet.GetComponent<Outlet>();
        o.PlugInLight(this.gameObject);

        //Game Manager verifies plug order
        GameManager.CheckPlugOrder();

    }

    //Disassocaites light from outlet (not plugged in)
    public void UnPlugLight()
    {
        lightRenderer.material = lightOff; 
        pluggedIn = false; 
    }
}
