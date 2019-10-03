using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    // Unity UI public vars

    /// <summary>
    /// A set of Outlet GameObjects. When all outlets are powered with a connected light
    /// the spawn will stop producing enemy units.
    /// </summary>
    public List<GameObject> ControlOutlets;

    /// <summary>
    /// The frequency at which the spawn produced enemies. 
    /// </summary>
    public float SpawnTime;

    /// <summary>
    /// GameObject what will be produced. 
    /// </summary>
    public GameObject Enemy;

    /// <summary>
    /// Indicates the radius around the spawn in which the enemies can be produced.
    /// </summary>
    public float Range;

    // Public Properties

    /// <summary>
    /// True if spawn is producing units
    /// </summary>
    public bool IsEnabled { get; private set; }

    /// <summary>
    /// True if all outlets associated with a light are turned on.
    /// </summary>
    public bool LightsAreOn { get; private set;}

    // Private Vars

    /// <summary>
    /// Tracks total time elapsed for spawning enemies. 
    /// </summary>
    private float timer;
    
    // Start us called before the first frame.
    void Start()
    {
        timer = 0f;
        IsEnabled = false;
        LightsAreOn = false; 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        updateLightStatus();
        if (IsEnabled && !LightsAreOn) {
            Produce();
        }

    }

    /// <summary>
    /// Turns the spawn on or off based on the previous state
    /// </summary>
    public void ToggleEnabled() {
        IsEnabled = !IsEnabled;
    }

    /// <summary>
    /// Resets the spawn's timer to 0.
    /// </summary>
    public void ResetSpawn()
    {
        timer = 0.0f; 
    }

    // Private functions 

    /// <summary>
    /// Produces one enemy as the spawn's location
    /// </summary>
    private void Produce()
    {
        if (timer >= SpawnTime && IsEnabled)
        {
            timer = 0f;

            // Get Random point inside range and set height.
            Vector2 xzCoord = getXZCooridnates();

            //Debug.Log(xzCoord);

            float yScale = (float) System.Math.Round(Enemy.transform.localScale.y, 2);
            float y = Enemy.transform.localScale.y / 2.00f;

            Vector3 placementDifferential = new Vector3(xzCoord[0],0,xzCoord[1]);

            Vector3 position = this.transform.position + placementDifferential;
            position.y = y;

            // Place enemy on the map.
            Instantiate(Enemy, position, this.transform.rotation);
        }
    }

    /// <summary>
    /// Disables the spawn if all associated outlets are connected to lights with power.
    /// </summary>
    private void updateLightStatus()
    {
        // If no lights are connected then lights cannot be on.
        if(ControlOutlets.Count <= 0)
        {
            LightsAreOn = false;
            return;
        }

        // Store previous light status
        bool priorLightStatus = LightsAreOn;

        //Assume all lights are on
        LightsAreOn = true;

        // If light is found to be off, set lights are on to false
        foreach(GameObject outletOject in ControlOutlets)
        {
            Outlet outletScript = outletOject.GetComponent<Outlet>();
            if (!outletScript.IsLightOn)
            {
                LightsAreOn = false;
                continue;
            }
        }

        // Reset spawn timer if spawn is re-enabled
        if(!LightsAreOn && priorLightStatus)
        {
            ResetSpawn();
        }
    }

    /// <summary>
    /// Returns a random set of cooridnates inside a unit cicle multiplied by the Spawn's range.
    /// </summary>
    /// <returns></returns>
    private Vector2 getXZCooridnates()
    {
        return Random.insideUnitCircle * Range;
    }
}

