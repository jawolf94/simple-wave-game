﻿using UnityEngine;

/// <summary>
/// Class which defines all variables and functions of an outlet. 
/// All game objects functioning as an outlet must have this script atteched.
/// </summary>
public class Outlet : MonoBehaviour
{
    //Public Properties

    /// <summary>
    /// Bool set to true if light is plugged into an outlet.
    /// </summary>
    public bool IsPluggedInto
    {
        get
        {
            return pluggedInLight != null;
        }

        private set { }
    }

    /// <summary>
    /// Returns true if power is supplied to the outlet
    /// </summary>
    public bool HasPower
    {
        get
        {
            if (connectedSwitchScript != null)
            {
                return connectedSwitchScript.SwitchOn;
            }
            else
            {
                return true;
            }
        }

        private set { }
    }

    /// <summary>
    /// Returns true if a light is connected and has power.
    /// </summary>
    public bool IsLightOn
    {
        get
        {
            return IsPluggedInto && HasPower;
        }

        private set { }
    }

    /// <summary>
    /// Switch Connected to this outlet.
    /// </summary>
    private GameObject connectedSwitch;
    private Switch connectedSwitchScript;
    public GameObject ConnectedSwitch {

        get { return connectedSwitch; }

        set {
            connectedSwitch = value;
            connectedSwitchScript = ConnectedSwitch.GetComponent<Switch>();
        }
    }


    // Private instance vars

    /// <summary>
    /// Light Object currently plugged into the outlet
    /// </summary>
    private GameObject pluggedInLight;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>    
    void Start()
    {
        // Light are not initally plugged into outlets
        pluggedInLight = null;
    }

    /// <summary>
    /// Provide Power to the outlet.
    /// </summary>
    public void PowerOn()
    {
        // If the outlet has a connected light, power on the light.
        if(IsPluggedInto){
            PluggableLight light = pluggedInLight.GetComponent<PluggableLight>();
            light.PowerOn();
        }
    }

    /// <summary>
    /// Remove Power from outlet
    /// </summary>
    public void PowerOff()
    {
        // If the outlet has a connected light, turn it off.
        if (IsPluggedInto)
        {
            PluggableLight light = pluggedInLight.GetComponent<PluggableLight>();
            light.PowerOff();
        }
    }

    /// <summary>
    /// Function that updates object to reflect a plugged in state.
    /// </summary>
    /// <param name="light">The light being plugged in.</param>
    public void PlugInLight(GameObject light)
    {
        pluggedInLight = light; 
    }

    /// <summary>
    /// Function that updates object to reflect an unplugged state.
    /// </summary>
    public void UnPlugLight()
    {
        // Unplug light if light object is associated.
        if (IsPluggedInto)
        {
            PluggableLight light = pluggedInLight.GetComponent<PluggableLight>();
            light.UnPlugLight();
        }

        // Set Properties and private vars to refelct unplugged state.
        pluggedInLight = null;
    }

}
