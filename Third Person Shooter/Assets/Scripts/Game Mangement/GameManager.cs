using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text WaveText;
    public Text TimerText;
    public Text GameOverText;

    public int EnemyCount { get; private set; }
    public int PlayerCount { get; private set; }

    public float WaveTime;



    private int waveNumber;
    private float coolDownRemaining;
    [Tooltip("In Minutes")]
    public float WaveCoolDown;

    
    [Tooltip("In Minutes")]

    private float waveTimeRemaining;

    private GameObject [] spawnPoints;

    //Sorted dictionary that maps outlet order (int) to outlet (value)
    private SortedDictionary<int,GameObject> outlets;

    private bool isCoolDown;

    private float timer;
        
    // Start is called before the first frame update
    void Start()
    {
        EnemyCount = 0;
        waveNumber = 0;
        updateWaveNum();

        timer = 0.0f;
        WaveCoolDown = WaveCoolDown * 60.0f;
        WaveTime = WaveTime * 60.0f;

        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        stopSpawn();

        //Initialize outlet dictionary to be populated by all GameObjects with the outlet tag
        outlets = new SortedDictionary<int, GameObject>();
        setOutletOrder();


        isCoolDown = true;
        GameOverText.enabled = false; 

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (PlayerCount == 0) {
            DisplayGameOver();
        }

        if (isCoolDown)
        {
            coolDownRemaining = WaveCoolDown - timer;
            if (coolDownRemaining <= 0)
            {
                incWaveNum();
                coolDownRemaining = WaveCoolDown;
                timer = 0;
                isCoolDown = false;
                startWave();
            }

        }
        else
        {
            waveTimeRemaining = WaveTime - timer;
            if (waveTimeRemaining <= 0)
            {

                stopSpawn();
                waveTimeRemaining = 0;
                if (EnemyCount == 0){
                    timer = 0;
                    isCoolDown = true;
                    TimerText.color = Color.white;
                }
                else{
                    TimerText.color = Color.red;
                }
               
            }
        }

        updateTimerText();
        checkEndGame();

    }

    public void AddEnemy() {
        EnemyCount++;
    }

    public void RemoveEnemy() {
        EnemyCount--;
    }

    public void AddPlayer()
    {
        PlayerCount++;
    }

    public void RemovePlayer()
    {
        PlayerCount--;
    }

    //Check all outlets to determine they are plugged in in the correct order
    //If not unplug all lights
    public void CheckPlugOrder()
    {
        //Boolean tracking if the correct plug in order was maintained.
        bool orderPreserved = true;

        //Bool tracking if unplugged light was found
        bool foundUnplugged = false;

        foreach(int key in outlets.Keys)
        {
            Outlet outletScript = outlets[key].GetComponent<Outlet>();
            
            //If a an unplugged light has not yet been found and the current outlet has no light
            //Set unplugged to true
            if(!foundUnplugged && !outletScript.IsPluggedInto)
            {
                foundUnplugged = true; 
            }

            //If unplugged light was found and the current outlet has a light then order is not preserved
            if (foundUnplugged && outletScript.IsPluggedInto) {
                orderPreserved = false;
                break; 
            }
        }

        //If the order was not preserved then unplug all lights
        if (!orderPreserved)
        {
             unPlugAllLights();
        }
       
    }


    private void stopSpawn()
    {
        foreach (GameObject spawn in spawnPoints) {
     
            EnemySpawn spawnScript = spawn.GetComponent<EnemySpawn>();
            if (spawnScript.IsEnabled) {
                spawnScript.ToggleEnabled();
            }

            spawnScript.ResetSpawn();
        }
    }

    private void startWave() {
        foreach (GameObject spawn in spawnPoints)
        {
            EnemySpawn spawnScript = spawn.GetComponent<EnemySpawn>();
            if (!spawnScript.IsEnabled)
            {
                spawnScript.ToggleEnabled();
            }
        }
    }

    private void incWaveNum() {
        waveNumber++;
        updateWaveNum();
    }

    private void updateWaveNum() {
        string newText = string.Format("Wave: {0:00}", waveNumber);
        WaveText.text = newText;
    }

    private void updateTimerText() {
        float displayTime = isCoolDown ? coolDownRemaining : waveTimeRemaining;
        string displayText = isCoolDown ? "Wave Start" : "Wave Remaining";

        TimeSpan t = TimeSpan.FromSeconds(displayTime);
        string newText = string.Format(displayText + " - {0:D2}:{1:D2}",
            t.Minutes,
            t.Seconds);

        TimerText.text = newText;
    }

    private void DisplayGameOver() {
        GameOverText.enabled = true; 
    }

    //Gets all GameObjects tagged Outlet and adds them to the outlet dictionary by PlugOrder
    private void setOutletOrder()
    {
        GameObject[] taggedOutlets = GameObject.FindGameObjectsWithTag("Outlet");
        foreach(GameObject obj in taggedOutlets)
        {
            Outlet newOutlet = obj.GetComponent<Outlet>();
            if(newOutlet != null)
            {
                outlets.Add(newOutlet.PlugOrder, obj);
            }
        }
    }

    //Calls each outlet to unplug associated lights
    private void unPlugAllLights()
    {
        foreach(GameObject outlet in outlets.Values)
        {
            Outlet outletScript = outlet.GetComponent<Outlet>();
            outletScript.UnPlugLight();
        }
    }

    /// <summary>
    /// A function that checks to see if a player quit the game.
    /// </summary>
    private void checkEndGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            endGame();
        }
    }
    /// <summary>
    /// A function that exits the current game
    /// </summary>
    private void endGame()
    {
        Application.Quit();
    }


}
