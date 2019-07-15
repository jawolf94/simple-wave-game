using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsWeapons : MonoBehaviour, IPlayerAction
{
    //Public Settings
    public float CoolDown;
    public float ShotRange;
    public float ShotDamage;

    //Properties

    /// <summary>
    /// The Player controller referencing this object. 
    /// </summary>
    /// 
    public PlayerController PlayerController { get; set; }

    /// <summary>
    /// True if the Player can use thier weapon.
    /// </summary>
    public bool ShotEnabled { get; set; }

    //Private vars
    
    /// <summary>
    /// Line representing the player's weapon action
    /// </summary>
    private LineRenderer shotLine;

    /// <summary>
    /// Layer Mask representing objects that can be hit by weapons
    /// </summary>
    private int shootableMask;

    /// <summary>
    /// The amount of time a player's weapon action will display
    /// </summary>
    private float shotDisplayTime = 0.3f;

    /// <summary>
    /// The amount of time accumulated since the weapon was last used.
    /// </summary>
    private float timer;

    
    // Start is called before the first frame update
    void Start()
    {
        //Timer starts at 0. 
        timer = 0;

        //Allow the player to use weapon at scene start
        ShotEnabled = true;

        //Layer Mask defining objects that are hitable by weapons
        shootableMask = LayerMask.GetMask("Shootable");

        //Connect Line Renderer Component
        shotLine = GetComponent<LineRenderer>();
        if (shotLine == null) {
            Debug.Log("[FireWeapon] Error: No Line Renderer attached to player object.");
        }
        shotLine.enabled = false;
    }
   
    /// <summary>
    /// Executes before the PlayerController checks for Player Input.
    /// All actions that should be done before the player performs an action should be placed here. 
    /// </summary>
    public void PreAction()
    {
        //Update the total time since the weapon was lasted used.
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Executes After the PlayerController checks for Player Input
    /// All actions that should be done after the player performs an action should be placed here.
    /// </summary>
    public void PostAction()
    {
        //Hide the weapon effect if 
        if (timer >= CoolDown * shotDisplayTime)
        {
            stopEffects();
        }
    }

    /// <summary>
    /// Function used to fire the player's weapon
    /// </summary>
    public void Fire()
    {
        //Check if weapon is enabled and cooldown period has passed.
        if (ShotEnabled && timer >= CoolDown)
        {
            fire();
        }
    }

    /// <summary>
    /// Stop displaying the weapon effects
    /// </summary>
    private void stopEffects() {
       shotLine.enabled = false; 
    }

    /// <summary>
    /// Uses the player's weapon.
    /// Function creates line render of weapon and checks for hit objects.
    /// </summary>
    private void fire() {

        //Reset the timer since last fired
        timer = 0;

        //Create the origin point of the weapon effect.
        shotLine.enabled = true;
        shotLine.SetPosition(0, transform.position);

        //Creates Ray used to determine what objects were hit by the weapon
        Ray shotRay = new Ray();
        RaycastHit shotHit;
        shotRay.origin = transform.position;
        shotRay.direction = transform.forward;

        //Check if the Ray hit any objects in the Shootable Mask
        if (Physics.Raycast(shotRay, out shotHit, ShotRange, shootableMask))
        {
            //Hit

            //Sets endpoint of weapon effect to the position of the hit object
            shotLine.SetPosition(1, shotHit.point);

            //Assess damage on the hit GameObject
            Shootable targetHit = shotHit.collider.GetComponent<Shootable>();
            targetHit.TakeDamage(ShotDamage);
            if (!targetHit.IsAlive) {
                PlayerController.WillPowerActions.ModifyWillPower(targetHit.WillPowerReward);
            }
        }
        else
        {
            //Displays shot effect to end of weapon range
            shotLine.SetPosition(1, shotRay.origin + shotRay.direction * ShotRange);
        }

    }

    /// <summary>
    /// Enables or Disables the Player's weapon.
    /// </summary>
    /// <param name="onOff">Bool - True if player can use weapon</param>
    public void ToggleShot(bool onOff)
    {
        ShotEnabled = onOff;
    }
}
