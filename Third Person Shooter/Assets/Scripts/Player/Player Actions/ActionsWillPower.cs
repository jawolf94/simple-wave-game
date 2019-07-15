using UnityEngine;
using UnityEngine.UI;

public class ActionsWillPower: MonoBehaviour, IPlayerAction
{
    //Unity UI set vars

    /// <summary>
    /// UI element which displays the player's total WP earned
    /// </summary>
    public Text WillPowerText;

    /// <summary>
    /// Text used to display the player's current level.
    /// </summary>
    public Text LevelText;

    //Properties

    /// <summary>
    /// The Player controller referencing this object. 
    /// </summary>
    /// 
    public PlayerController PlayerController { get; set; }

    /// <summary>
    /// Sum of all WP earned by the Player.
    /// </summary>
    public int TotalWillPowerEarned { get; private set; }

    /// <summary>
    /// The Player's current level
    /// </summary>
    public int PlayerLevel
    {
        get
        {
            return levelProgress.GetCurrentLevel();
        }

        private set { }
    }

    //Private variables

    /// <summary>
    /// Object used to track the Player's current level based on the total WP earned.
    /// </summary>
    private LevelProgress levelProgress;

    /// <summary>
    /// String defining what is displayed on the UI for will power.
    /// </summary>
    private static readonly string willPowerText = "Will Power: {0}";

    //Functions

    //Start is called before the first frame update
    public void Start()
    {
        //WP always starts at 0 when the player begins the game
        this.TotalWillPowerEarned = 0;

        //Sets the UI Text Element to display the starting value
        updateExpText();


        //Create level progress object to calc player's level
        this.levelProgress = ScriptableObject.CreateInstance(typeof(LevelProgress)) as LevelProgress;
        this.levelProgress.Init(LevelText);
    }

    /// <summary>
    /// Executes before the PlayerController checks for Player Input.
    /// All actions that should be done before the player performs an action should be placed here. 
    /// </summary>
    public void PreAction()
    {
        return;
    }

    /// <summary>
    /// Executes After the PlayerController checks for Player Input
    /// All actions that should be done after the player performs an action should be placed here.
    /// </summary>
    public void PostAction()
    {
        return;
    }

    /// <summary>
    /// A function that modifies a player's total WP by the change value
    /// </summary>
    /// <param name="changeValue">Amount by which to change the Player's WP</param>
    public void ModifyWillPower(int changeValue)
    {
        TotalWillPowerEarned += changeValue;
        updateExpText();
        levelProgress.UpdateLevelIndex(TotalWillPowerEarned);
    }

    /// <summary>
    /// Private function to update the WP UI Text. Should be called whenever the WP value changes. 
    /// </summary>
    private void updateExpText()
    {
        string newText = string.Format(willPowerText, TotalWillPowerEarned);
        WillPowerText.text = newText;
    }

}
