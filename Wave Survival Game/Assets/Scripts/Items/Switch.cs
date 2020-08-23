using UnityEngine;

/// <summary>
/// Defines Light Swtich object behavior and state.
/// </summary>
public class Switch : MonoBehaviour
{

    // Vars set by Unity Editor

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

    // Private instance vars

    /// <summary>
    /// Script for the connected outlet.
    /// </summary>
    private Outlet outletScript;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
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

    // Public Funtions
    
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
