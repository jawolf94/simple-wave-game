using UnityEngine;
using UnityEngine.UI;

public class WillPowerTracker: MonoBehaviour
{
    //Properties

    /// <summary>
    /// Sum of all WP earned by the Player.
    /// </summary>
    public int TotalWillPowerEarned
    {
        get;
        private set;
    }

    /// <summary>
    /// The Player's current level
    /// </summary>
    public int PlayerLevel
    {
        get
        {
            return levels.GetCurrentLevel();
        }
        private set { }
    }

    //Private variables

    /// <summary>
    /// UI element which displays the player's total WP earned
    /// </summary>
    private Text uiWillPoweText;

    /// <summary>
    /// Object used to track the Player's current level based on the total WP earned.
    /// </summary>
    private LevelProgress levels;

    /// <summary>
    /// String defining what is displayed on the UI for will power.
    /// </summary>
    private static readonly string willPowerText = "Will Power: {0}";

    //Functions

   /// <summary>
   /// Function used to initialize a WP tracker. Function will create a new object with no WP earned. 
   /// </summary>
   /// <param name="expText">Text field on the UI used to display a player's WP stats</param>
   /// <param name="levelText">Text field on the UI used to disp;at a player's current level</param>
    public void Init(Text expText, Text levelText) {

        //WP always starts at 0 when the player begins the game
        this.TotalWillPowerEarned = 0;

        //Sets the UI Text Element to display the starting value
        this.uiWillPoweText = expText;
        updateExpText();


        //Create level progress object to calc player's level
        this.levels = ScriptableObject.CreateInstance(typeof(LevelProgress)) as LevelProgress;
        this.levels.Init(levelText);
    }
    
   /// <summary>
   /// A function that modifies a player's total WP by the change value
   /// </summary>
   /// <param name="changeValue">Amount by which to change the Player's WP</param>
    public void ModifyWillPower(int changeValue)
    {
        TotalWillPowerEarned += changeValue;
        updateExpText();
        levels.UpdateLevelIndex(TotalWillPowerEarned);
    }

    /// <summary>
    /// Private function to update the WP UI Text. Should be called whenever the WP value changes. 
    /// </summary>
    private void updateExpText()
    {
        string newText = string.Format(willPowerText, TotalWillPowerEarned);
        uiWillPoweText.text = newText;
    }

}
