using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to Spawn GameObjects. Handles production of enemies in game. 
/// </summary>
public class EnemySpawn : MonoBehaviour
{

    // Vars set in Unity Editor

    /// <summary>
    /// A set of Outlet GameObjects. When all outlets are powered with a connected light
    /// the spawn will stop producing enemy units.
    /// </summary>
    public List<GameObject> ControlOutlets;

    /// <summary>
    /// The frequency at which the spawn produces enemies. 
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

    /// <summary>
    /// Start us called before the first frame.
    /// </summary>
    void Start()
    {
        timer = 0f;
        IsEnabled = false;
        LightsAreOn = false; 
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>    
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
    /// Produces one enemy at the spawn's location
    /// </summary>
    private void Produce()
    {
        // Check is enough time has passed to produce a new enemy and if spawn is enabled
        if (timer >= SpawnTime && IsEnabled)
        {
            timer = 0f;

            // Get Random point inside range
            Vector2 xzCoord = getXZCooridnates();

            // Calculate y (height) based on Enemy's size
            float yScale = (float) System.Math.Round(Enemy.transform.localScale.y, 2);
            float y = Enemy.transform.localScale.y / 2.00f;

            // Vector3 representing the difference between spawn's location and placement location.
            Vector3 placementDifferential = new Vector3(xzCoord[0],0,xzCoord[1]);

            // Set xyz location
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

        // Assume all lights are on
        LightsAreOn = true;

        // If light is found to be off then negate assumption. 
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
    /// Generates a random set of cooridnates inside a unit cicle multiplied by the Spawn's range.
    /// </summary>
    /// <returns> Vector2 representing a location within the spawn's range.</returns>
    private Vector2 getXZCooridnates()
    {
        return Random.insideUnitCircle * Range;
    }
}

