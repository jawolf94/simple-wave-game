using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress: ScriptableObject
{
    // Private variables

    /// <summary>
    /// Unity UI Text that displays the Player's current level.
    /// </summary>
    private Text levelText;

    /// <summary>
    /// List of Level structs defining a player's leveling structure.
    /// </summary>
    private List<Level> levels;

    /// <summary>
    /// An index referencing the Player's current level struct.
    /// </summary>
    private int curLevelIndex;

    //Functtions

    /// <summary>
    /// Initializes a Player's Level tiering.
    /// </summary>
    /// <param name="levelText">Unity UI Text to display player's current level.</param>
    public void Init(Text levelText) {

        // Set the player's starting level index.
        this.curLevelIndex = 0;

        // Assign and update the player's level text.
        this.levelText = levelText;
        updateLevelText();

        // Define arbitrary leveling structure - will be flushed out in later featuers
        // next level = sum(all prior required will power) + current level#
        int sumWillPower = 0;
        this.levels = new List<Level>();

        for (int i = 0; i < 5; i++)
        {
            sumWillPower += i + 1;
            Level newLevel = new Level();
            newLevel.RequiredWillPower = sumWillPower;
            levels.Add(newLevel);
        }
    }

    /// <summary>
    /// Updates the level index (current level) of the player if awarded enough will power.
    /// </summary>
    /// <param name="totalWillPower">The Player's total willpower</param>
    /// <returns>Bool - True if the player's level was updated</returns>
    public bool UpdateLevelIndex(int totalWillPower) {

        // Get the Player's next level index and determine if another level tier exists.
        int nextLevelIndex = curLevelIndex + 1;
        if (nextLevelIndex >= levels.Count) {
            // No further levels to progress to.
            return false;
        }
        else{
            // Next level exists
            Level nextLevel = levels[nextLevelIndex];

            // Check if player has earned enough Will Power to progress to next level.
            if (totalWillPower < nextLevel.RequiredWillPower)
            {
                // Player has not earned enough Will Power.
                return false;
            }
            else {
                // Player has eanred enough Will Power. Update Level references.
                curLevelIndex++;
                updateLevelText();
                return true;
            }
        }
    }

    /// <summary>
    /// Gets the player's current level.
    /// </summary>
    /// <returns>Int - Player's current level</returns>
    public int GetCurrentLevel() {
        return curLevelIndex + 1;
    }

    /// <summary>
    /// Updates the UI Text filed to display the player's current level.
    /// </summary>
    private void updateLevelText() {
        string newText = string.Format(
            "Level: {0}",
            GetCurrentLevel());
        levelText.text = newText;
    }
   
}

/// <summary>
/// Struct representing a Player's level.
/// </summary>
public struct Level {

    /// <summary>
    /// Defines the amount of Will Power required to progress to this level.
    /// </summary>
    public int RequiredWillPower;

    //ToDo: Flush out information for level perks
}
