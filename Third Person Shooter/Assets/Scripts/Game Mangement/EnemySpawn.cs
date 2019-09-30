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
            Instantiate(Enemy, transform);
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
}

