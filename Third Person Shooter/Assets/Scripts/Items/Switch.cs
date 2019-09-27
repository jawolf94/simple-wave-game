using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    /*
     * Desc: Defines Light Swtich object behavior and state.
     */

    //Unity UI Public Vars

    /// <summary>
    /// Outlet controlled by this swtich.
    /// </summary>
    public GameObject ConnectedOutlet;

    //Public Properties

    /// <summary>
    /// True if switch is in the On State
    /// </summary>
    public bool SwitchOn
    {
        get;
        private set;
    }

    //private instance vars

    /// <summary>
    /// Script for the connected outlet.
    /// </summary>
    private Outlet outletScript;

    // Start is called before the first frame update
    void Start()
    {
        // Switches start in off state
        SwitchOn = false;

        // Set ConnectedSwtich property on ConnectedOutlet
        if (ConnectedOutlet != null)
        {
            outletScript = ConnectedOutlet.GetComponent<Outlet>();
            outletScript.ConnectedSwitch = this.gameObject;
        }
        else
        {
            Debug.Log("[" + this.name + "]: No outlet is associated with this outlet.");
        }
    }

    //Public Funtions
    
    /// <summary>
    /// Toggles the value of SwtichOn and updates the assoicated outlet
    /// </summary>
    public void ToggleSwitch()
    {
        // Flip the state of the switch.  
        SwitchOn = !SwitchOn;

        // Update outlet state depending on the switches current state. 
        if (SwitchOn)
        {
            outletScript.PowerOn();
        }
        else
        {
            outletScript.PowerOff();
        }
    }
}
