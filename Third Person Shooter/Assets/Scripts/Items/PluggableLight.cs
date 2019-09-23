﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluggableLight : MonoBehaviour
{
    /*Desc: Class which defines all variables and functions of a light. 
     *All game objects functioning as a light must have this script atteched.
    */

    //Pubic vars set by Unity UI

    /// <summary>
    /// Material set when light is on.
    /// </summary>
    public Material lightOn;

    /// <summary>
    /// Material set when light is off. 
    /// </summary>
    public Material lightOff;

    //Public Properties

    /// <summary>
    /// True if light is in range of an outlet
    /// </summary>
    private bool outletInRange;
    public bool OutletInRange {
        get { return outletInRange; }
        private set { outletInRange = value;}
    }

    /// <summary>
    /// True if light is plugged in to an outlet 
    /// </summary>
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

    //Instance variables

    /// <summary>
    /// Instance of GameManager
    /// </summary>
    private GameManager GameManager;

    /// <summary>
    /// Renderer to update material
    /// </summary>
    private Renderer lightRenderer;

    /// <summary>
    /// Light Component to produce scene lighting
    /// </summary>
    private Light lightComponent;

    /// <summary>
    /// GameObject of nearby outlet. Null if not in range 
    /// </summary>
    private GameObject nearbyOutlet;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Find scene's instance of Game Manager
        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        //Get attached Renderer component. Lights are initially off
        lightRenderer = GetComponent<Renderer>();
        lightRenderer.material = lightOff;

        //Get Light Compoenent to control scene lighting. Set to Off
        lightComponent = GetComponent<Light>();
        lightComponent.enabled = false; 
        
        OutletInRange = false;
        nearbyOutlet = null;
        
    }

    /// <summary>
    /// Called when this object collider overlapps another object's collider area. 
    /// </summary>
    /// <param name="other">The other object's collider<</param>
    public void OnTriggerEnter(Collider other)
    {
        //Set light in range if it enters outlet trigger
        if (other.tag.Equals("Outlet"))
        {
            OutletInRange = true;
            nearbyOutlet = other.gameObject;
        }
    }

    /// <summary>
    /// Called when this object collider leaves another object's collider area. 
    /// </summary>
    /// <param name="other">The other objects collider</param>
    public void OnTriggerExit(Collider other)
    {
        //Set light out of ranfe if it exits outlet trigger
        if (other.tag.Equals("Outlet"))
        {
            OutletInRange = false;
            nearbyOutlet = null;
        }

    }

    /// <summary>
    /// Sets Light object to a plugged in state
    /// </summary>
    public void PlugInLight()
    {
        //Light is flagged as plugged in
        PluggedIn = true;

        //Material set to on material 
        lightRenderer.material = lightOn;

        //Enable Scene lighting
        lightComponent.enabled = true;

        //Update Outlet object
        Outlet o = nearbyOutlet.GetComponent<Outlet>();
        o.PlugInLight(this.gameObject);

        //Game Manager verifies all lights are plugged in
        GameManager.IsVictory();

    }

    /// <summary>
    /// Sets light to off state and disassociates the light with the outlet
    /// </summary>
    public void UnPlugLight()
    {
        //Set Light Material to Off Material.
        lightRenderer.material = lightOff;

        //Turn off scene lighting.
        lightComponent.enabled = false; 
        
        //Set state flags to false.
        pluggedIn = false; 
    }
}
