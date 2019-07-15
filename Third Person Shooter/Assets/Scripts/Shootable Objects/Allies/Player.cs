using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Shootable
{
    //Unity UI set variables

    /// <summary>
    /// UI Text field that displays the user's current sanity
    /// </summary>
    public Text SanityText;

    //Private Variables

    /// <summary>
    /// String which is displayed in the UI Text field.
    /// </summary>
    private string sanityTextString = "Sanity: {0}";

    // Start is called before the first frame update
    new void Start()
    {
        //Call Shootable Start function to initalize health values.
        base.Start();

        //Set sanity UI text to starting health value.
        updateSanityText(Health);

        //Inform GameManager of new Player.
        GameManager.AddPlayer();

        
    }

    /// <summary>
    /// Function which defines a Player Object's behavior on each frame update.
    /// </summary>
    public override void UpdateBehavior()
    {
        base.UpdateBehavior();

    }


    /// <summary>
    /// Function which defines how to destroy the Player object when killed. 
    /// </summary>
    public override void DestroyShootable()
    {
        base.DestroyShootable();
        GameManager.RemovePlayer();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Function that applies damage taken to sthe Player Object.
    /// </summary>
    /// <param name="damageTaken">The amount of damage to subtract from the Player's health. </param>
    public override void TakeDamage(float damageTaken)
    {
        base.TakeDamage(damageTaken);
        updateSanityText(Health);
    }

    /// <summary>
    /// Update's the Player's sanity text. Should be called whenever the sanity (health) of a player is updated. 
    /// </summary>
    /// <param name="value">New sanity value to display</param>
    private void updateSanityText(float value)
    {
        SanityText.text = string.Format(
            sanityTextString,
            value);
    }





}
