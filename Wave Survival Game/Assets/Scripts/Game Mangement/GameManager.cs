﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to manage the state of the game. Tracks players, enemies, and victory conditions.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Vars set in Unity Editor

    /// <summary>
    /// Text to Display wave number.
    /// </summary>
    public Text WaveText;

    /// <summary>
    /// Text to display wave and cool down timer.
    /// </summary>
    public Text TimerText;

    /// <summary>
    /// Text that displays when level ends.
    /// </summary>
    public Text EndLevelText;

    /// <summary>
    /// Color of EndLevelText when the player wins.
    /// </summary>
    public Color winColor;

    /// <summary>
    /// Color of EndLevelText when the player loses.
    /// </summary>
    public Color loseColor;

    /// <summary>
    /// Game object containing clickable menu actions.
    /// </summary>
    public GameObject MenuTextContainer;

    /// <summary>
    /// Amount of time for each wave.
    /// </summary>
    public float WaveTime;

    /// <summary>
    /// Amount of time between waves.
    /// </summary>
    public float WaveCoolDown;

    // Public Properties

    /// <summary>
    /// The number of enemies alive.
    /// </summary>
    public int EnemyCount { get; private set; }

    /// <summary>
    /// The number of players alive.
    /// </summary>
    public int PlayerCount { get; private set; }


    //Private instance vars

    /// <summary>
    /// Total elapsed time in the wave or cooldown.
    /// </summary>
    private float timer;

    /// <summary>
    /// Total number of waves passed.
    /// </summary>
    private int waveNumber;

    /// <summary>
    /// Amount of time left in the wave.
    /// </summary>
    private float waveTimeRemaining;

    /// <summary>
    /// Amount of time left in the cool down.
    /// </summary>
    private float coolDownRemaining;

    /// <summary>
    /// True if currently in between waves.
    /// </summary>
    private bool isCoolDown;
  
    /// <summary>
    ///  All Spawn points in the current level.
    /// </summary>
    private GameObject [] spawnPoints;

    /// <summary>
    /// Dictionary that maps light GameObjects to thier scripts
    /// </summary>
    private Dictionary<GameObject, PluggableLight> lights;

    // Constant strings for UI Display.

    /// <summary>
    /// Text to display when the player loses. 
    /// </summary>
    private static string loseLevelText = "Game Over";

    /// <summary>
    /// Text to display when the player wins.
    /// </summary>
    private static string winLevelText = "Victory!";

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Game starts with no enemies or players.
        EnemyCount = 0;
        waveNumber = 0;

        // Set wave number textto inital values.
        updateWaveNum();

        // Set timer to 0 and convert minutes (entered via UI) to seconds.
        timer = 0.0f;
        WaveCoolDown = WaveCoolDown * 60.0f;
        WaveTime = WaveTime * 60.0f;

        // Get all spawn points and prevent them from producing enemies
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        stopSpawn();
        isCoolDown = true;

        // Initialize light dictionary to be populated by all GameObjects with the "Light" tag
        lights = new Dictionary<GameObject, PluggableLight>();
        findAllLights();

        // Hide end level text & menu actions
        EndLevelText.enabled = false; 
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Increment time
        timer += Time.deltaTime;

        // Check if any players are still alive
        if (PlayerCount == 0) {
            DisplayLevelLose();
        }

        // If level is in cool down period
        if (isCoolDown)
        {
            // Update remaining time in cooldown
            coolDownRemaining = WaveCoolDown - timer;

            // If cooldown period has ended
            if (coolDownRemaining <= 0)
            {
                // Increase the wave
                incWaveNum();

                // Reset timers
                coolDownRemaining = WaveCoolDown;
                timer = 0;

                // Start wave
                isCoolDown = false;
                startWave();
            }

        }
        // If wave is ongoing
        else
        {

            // Update the time remaining in the wave
            waveTimeRemaining = WaveTime - timer;

            // If the wave has ended
            if (waveTimeRemaining <= 0)
            {
                // Disable spawns and reset timer
                stopSpawn();
                waveTimeRemaining = 0;

                // If all enemies are dead, start cooldown
                if (EnemyCount == 0){
                    timer = 0;
                    isCoolDown = true;
                    TimerText.color = Color.white;
                }
                else{
                    // If enemies remain keep wave active
                    TimerText.color = Color.red;
                }
               
            }
        }

        // Update timer text to display remaining time
        updateTimerText();

        // Check is the player has plugged in all outlets
        checkIfPlayerStop();
    }

    // Public functions

    /// <summary>
    /// Add enemy to the overall count
    /// </summary>
    public void AddEnemy() {
        EnemyCount++;
    }

    /// <summary>
    /// Remove enemy from the overall count
    /// </summary>
    public void RemoveEnemy() {
        EnemyCount--;
    }

    /// <summary>
    /// Add player to the overall count.
    /// </summary>
    public void AddPlayer()
    {
        PlayerCount++;
    }

    /// <summary>
    /// Remove player from the overall count
    /// </summary>
    public void RemovePlayer()
    {
        PlayerCount--;
    }

    /// <summary>
    /// Function to check if the player won. Executues UI update if player wins.
    /// </summary>
    public void Victory()
    {
        // Assume vistory is true
        bool victory = true;

        // Loop through all pluggable lights. If any are off, victory is false
        foreach (PluggableLight light in lights.Values)
        {            
            if (!light.IsTurnedOn)
            {
                victory = false;
            }
        }

        // If the player has won, display victory menu + text
        if (victory)
        {
            DisplayLevelWin();
        }
    }

    // Private functions

    /// <summary>
    /// Function to set and display win message on UI.
    /// </summary>
    private void DisplayLevelWin()
    {
        // Set text string to equal winning const
        EndLevelText.text = winLevelText;

        // Set color 
        EndLevelText.color = winColor;

        // Display text and menu options
        EndLevelText.enabled = true;
        showMenuOptions();
    }

    /// <summary>
    /// Function to set and display losing message on UI.
    /// </summary>
    private void DisplayLevelLose()
    {
        // Set text string to losing const
        EndLevelText.text = loseLevelText;

        // Set text color
        EndLevelText.color = loseColor;

        // Displat text and menu options
        EndLevelText.enabled = true;
        showMenuOptions();
    }

    /// <summary>
    /// Function to Display clickable text for player
    /// </summary>
    private void showMenuOptions()
    {
        MenuTextContainer.SetActive(true);
    }

    /// <summary>
    /// Function to enable all spawns
    /// </summary>
    private void startWave()
    {
        // Loop through all Spawn Game Object
        foreach (GameObject spawn in spawnPoints)
        {
            // Get spawn stript component and enable if not yet enabled
            EnemySpawn spawnScript = spawn.GetComponent<EnemySpawn>();
            if (!spawnScript.IsEnabled)
            {
                spawnScript.ToggleEnabled();
            }
        }
    }

    /// <summary>
    /// Function to disable all spawns
    /// </summary>
    private void stopSpawn()
    {
        // Loop through all spawn game objects
        foreach (GameObject spawn in spawnPoints) {
            
            // Get spawn script and disable if not yet disabled
            EnemySpawn spawnScript = spawn.GetComponent<EnemySpawn>();
            if (spawnScript.IsEnabled) {
                spawnScript.ToggleEnabled();
            }

            // Reset spawn timer to 0
            spawnScript.ResetSpawn();
        }
    }

   /// <summary>
   /// Function to increment the wave number and updates the UI text
   /// </summary>
    private void incWaveNum() {
        waveNumber++;
        updateWaveNum();
    }

    /// <summary>
    /// Function to update the UI text to display current wave number.
    /// </summary>
    private void updateWaveNum() {
        string newText = string.Format("Wave: {0:00}", waveNumber);
        WaveText.text = newText;
    }

    /// <summary>
    /// Function to update the UI text to display current time in wave or cooldown
    /// </summary>
    private void updateTimerText() {

        // Decide to display wave or cool down time reamining
        float displayTime = isCoolDown ? coolDownRemaining : waveTimeRemaining;
        string displayText = isCoolDown ? "Wave Start" : "Wave Remaining";

        // Create timespan from time remaining
        TimeSpan t = TimeSpan.FromSeconds(displayTime);

        // Format time string as minutes and seconds with proper label
        string newText = string.Format(displayText + " - {0:D2}:{1:D2}",
            t.Minutes,
            t.Seconds);

        // Display
        TimerText.text = newText;
    }

    /// <summary>
    /// Function which adds all GameObjects tagged "Light" to the light dictionary
    /// </summary>
    private void findAllLights()
    {
        // Find all objects with light tag.
        GameObject[] taggedLights = GameObject.FindGameObjectsWithTag("Light");

        // Loop through objects to add them to the set.
        foreach(GameObject obj in taggedLights)
        {
            lights.Add(obj, obj.GetComponent<PluggableLight>());
        }
    }

    /// <summary>
    /// A function that checks to see if a player quit the game.
    /// </summary>
    private void checkIfPlayerStop()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stopGame();
        }
    }
    /// <summary>
    /// A function that exits the current game
    /// </summary>
    private void stopGame()
    {
        SceneManager.LoadScene("Assets/Scenes/Main Menu.unity");
    }


}
