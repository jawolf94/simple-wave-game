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


}
