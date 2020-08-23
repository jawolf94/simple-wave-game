using UnityEngine;

/// <summary>
/// Class which defines all variables and functions of a light. 
/// All game objects functioning as a light must have this script atteched.
/// </summary>
public class PluggableLight : MonoBehaviour
{

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
    
    /// <summary>
    /// True if light has power and is illuminated.
    /// </summary>
    public bool IsTurnedOn {
        get;
        private set;
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
        // Find scene's instance of Game Manager
        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        // Get attached Renderer component. Lights are initially off
        lightRenderer = GetComponent<Renderer>();
        lightRenderer.material = lightOff;


        // Get Light Compoenent to control scene lighting. Set to Off
        lightComponent = GetComponent<Light>();
        lightComponent.enabled = false;

        // Lights do not start plugged in
        PluggedIn = false;
        IsTurnedOn = false;
        
        // Assume light is not next to an outlet
        OutletInRange = false;
        nearbyOutlet = null;
        
    }

    /// <summary>
    /// Called when this object collider overlaps another object's collider area. 
    /// </summary>
    /// <param name="other">The other object's collider<</param>
    public void OnTriggerEnter(Collider other)
    {
        // Set light in range of outlet if it enters outlet trigger
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
        // Set light out of range if it exits outlet trigger
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
        // Light is flagged as plugged in
        PluggedIn = true;

        // Update Outlet object
        Outlet o = nearbyOutlet.GetComponent<Outlet>();
        o.PlugInLight(this.gameObject);

        // Check if outlet has power
        if (o.HasPower)
        {
            PowerOn();
        }
    }


    /// <summary>
    /// Turns on the light and sets object state. Triggers check for victory.
    /// </summary>
    public void PowerOn()
    {
        IsTurnedOn = true; 

        // Update Light Object to appear on in Game
        setLightOnMaterial();

        // Game Manager verifies all lights are plugged in
        GameManager.Victory();
    }

    /// <summary>
    /// Turns off the light and sets object state
    /// </summary>
    public void PowerOff()
    {
        IsTurnedOn = false;

        // Update Light Object to appear off in Game
        setLightOffMaterial();
    }

    /// <summary>
    /// Sets light to off state and disassociates the light with the outlet
    /// </summary>
    public void UnPlugLight()
    {
        // Set state flags to false.
        pluggedIn = false;

        PowerOff();
    }

    // Private Functions

    /// <summary>
    /// Sets light material to "On" and enables scene lighting
    /// </summary>
    private void setLightOnMaterial()
    {
        //Material set to "ON" material 
        lightRenderer.material = lightOn;

        //Enable Scene lighting
        lightComponent.enabled = true;
    }

    /// <summary>
    /// Sets light material to "Off" and disables scene lighting
    /// </summary>
    private void setLightOffMaterial()
    {
        //Set Light Material to Off Material.
        lightRenderer.material = lightOff;

        //Turn off scene lighting.
        lightComponent.enabled = false;
    }
}
