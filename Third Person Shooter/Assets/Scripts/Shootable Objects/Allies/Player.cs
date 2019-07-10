using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Shootable
{
    //Properties

    /// <summary>
    /// UI Text field that displays the user's current sanity
    /// </summary>
    public Text SanityText { get; set; }

    //Private Variables
    /// <summary>
    /// String which is displayed in the UI Text field
    /// </summary>
    private string sanityTextString = "Sanity: {0}";

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        GameManager.AddPlayer();
    }

    /// <summary>
    /// Function used to initalize the Player object. 
    /// </summary>
    /// <param name="sanity">Sanity represents a player's health. This is the starting amount for a new Player</param>
    /// <param name="willPower">The amount of will power earned from killing the Player</param>
    /// <param name="sanityText">The UI Text field to display a Player's sanity level</param>
    public void Init(int sanity, int willPower, Text sanityText)
    {
        StartingHealth = sanity;
        WillPowerReward = willPower;
        SanityText = sanityText;
        updateSanityText(StartingHealth); 
    }

    public override void UpdateBehavior()
    {
        base.UpdateBehavior();

    }

    public override void DestroyShootable()
    {
        base.DestroyShootable();
        GameManager.RemovePlayer();
        Destroy(this.gameObject);
    }


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
