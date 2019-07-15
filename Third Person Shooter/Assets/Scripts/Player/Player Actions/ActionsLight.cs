﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsLight : MonoBehaviour, IPlayerAction
{
    //Unity UI Set Vars

    /// <summary>
    /// UI Text Field used to display action's a player can take when near a light
    /// </summary>
    public Text LightActionText;

    //Properties

    /// <summary>
    /// The Player controller referencing this object. 
    /// </summary>
    public PlayerController PlayerController { get; set; }

    //Delegate and variable mapped to player Right-Click Action

    /// <summary>
    /// Defines a type of function that executes an action on Lights. 
    /// Function can differ depending on what the player is allowed to perform. 
    /// </summary>
    public delegate void LightAction();

    /// <summary>
    /// Variable representing the current action a player may perform on a light
    /// </summary>
    public LightAction PerformLightAction;

    //Private Vars

    /// <summary>
    /// True when the player is within range of a light object
    /// </summary>
    private bool lightInRange;

    /// <summary>
    /// GameObject of the light currently held by the Player
    /// </summary>
    private GameObject heldLight;

    /// <summary>
    /// GameObject of the light currnetly near the player
    /// </summary>
    private GameObject nearbyLight;


    //Strings that display when a player can preform an action on a light object
    private static readonly string pickUpText = "[RC] to Pick Up Light";
    private static readonly string dropText = "[RC] to Drop Light";
    private static readonly string plugText = "[RC] to Plug in Light";


    // Start is called before the first frame update
    void Start()
    {
        this.heldLight = null;
        this.nearbyLight = null;
        setLightInRange(false);
        PerformLightAction = null;
    }

    /// <summary>
    /// Executes before the PlayerController checks for Player Input.
    /// All actions that should be done before the player performs an action should be placed here. 
    /// </summary>
    public void PreAction()
    {
        setLightAction();
    }

    /// <summary>
    /// Executes After the PlayerController checks for Player Input
    /// All actions that should be done after the player performs an action should be placed here.
    /// </summary>
    public void PostAction()
    {
        moveLight();
    }

    //Function is called when a player enters a Trigger Collider
    public void OnTriggerEnter(Collider other)
    {
        //Set light object in range if player enters a light's radius
        if (other.tag.Equals("Light"))
        {
            setLightInRange(true);
            this.nearbyLight = other.gameObject;
        }
    }

    //Function is called when a player exits a Trigger Collider
    public void OnTriggerExit(Collider other)
    {
        //Deselct Light object if player leaves radius
        if (other.tag.Equals("Light"))
        {
            setLightInRange(false);
            this.nearbyLight = null;
        }
    }

  
    /// <summary>
    /// Sets the function a player can perform when a player is in range of a light.
    /// Function can either be pickUpLight(), dropLight(), plugInLight()
    /// </summary>
    private void setLightAction()
    {
        //Light is nearby and the player does not currently hold a light
        if (lightInRange && heldLight == null)
        {
            PluggableLight light = nearbyLight.GetComponent<PluggableLight>();

            //Nearby light is in range of an outlet
            if (light.OutletInRange)
            {
                //Light is not already plugged in
                if (!light.PluggedIn)
                {
                    //Plug In Light Action displays
                    PerformLightAction = plugInLight;
                    LightActionText.text = plugText;
                    LightActionText.enabled = true;

                }
                else
                {
                    //Light is already plugged in - no action can be taken
                    PerformLightAction = null;
                    LightActionText.text = string.Empty;
                    LightActionText.enabled = false;
                }
            }
            //Player is near a light not plugged in - can pick up
            else
            {
                //Pick Up the Light Action displays
                PerformLightAction = pickUpLight;
                LightActionText.text = pickUpText;
                LightActionText.enabled = true;
            }
        }
        //Player is holding a light
        else if (heldLight != null)
        {
            //Drop the light action displays for player
            PerformLightAction = dropLight;
            LightActionText.text = dropText;
            LightActionText.enabled = true;
        }
        //Player is not near a light
        else
        {
            LightActionText.text = string.Empty;
            LightActionText.enabled = false;
            PerformLightAction = null;
        }
    }

    /// <summary>
    /// Sets lightInRange to onOff
    /// </summary>
    /// <param name="onOff">Bool - True if light is in range</param>
    private void setLightInRange(bool onOff)
    {
        lightInRange = onOff;
    }

    /// <summary>
    /// Light Action Delegate.
    /// Sets light object triggered by player to player's inventory 
    /// </summary>
    private void pickUpLight()
    {
        this.heldLight = this.nearbyLight;
       PlayerController.WeaponActions.ToggleShot(false);
    }

    /// <summary>
    /// Light Action Delegate.
    /// Drops the light currently in the players inventory.  
    /// </summary>
    private void dropLight()
    {
        this.heldLight = null;
        PlayerController.WeaponActions.ToggleShot(true);
    }

    /// <summary>
    /// Light Action Delegate.
    /// Plugs in the light currently in nearby proximity to the player.
    /// </summary>
    private void plugInLight()
    {
        PluggableLight light = nearbyLight.GetComponent<PluggableLight>();
        light.PlugInLight();
    }

    /// <summary>
    /// Updates the held light's transform to stay infront of the player.
    /// </summary>
    private void moveLight()
    {
        if (this.heldLight != null)
        {
            //Set Initial Light position to the postion of the player
            Vector3 lightPos = this.transform.position;

            //Get the width of the light and half the width of the player
            float lightWidth = this.heldLight.transform.localScale.z / 2;
            float playerWidth = this.transform.localScale.z / 2;

            //Multiply the scalar vector of player's position by distance from player's center. 
            //Result = [X,0,0] where X is disantce in forward direction. 
            //Add to players position.
            lightPos += this.transform.forward * (lightWidth + playerWidth);

            //Set new light postion
            this.heldLight.transform.position = lightPos;

            //Rotate Light
            this.heldLight.transform.rotation = this.transform.rotation;
        }
    }
}