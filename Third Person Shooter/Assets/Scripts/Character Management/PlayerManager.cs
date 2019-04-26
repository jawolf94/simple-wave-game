using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int startingHealth;

    public Text experienceText;
    public Text levelText;
    public Text HealthText;

    public GameObject primaryPanel;
    public GameObject secondaryPanel;

    private GameObject playerObject;
    private Player playerInfo;
    private ExpTracker expTracker;
    private InventoryTracker invTracker;


    // Start is called before the first frame update
    void Start()
    {
        //Get Player object
        playerObject = GameObject.FindGameObjectWithTag("Player");

        //Add Player
        playerInfo = createComponent<Player>();
        playerInfo.Init(startingHealth, 0, HealthText);

        expTracker = createComponent<ExpTracker>();
        expTracker.Init(experienceText, levelText);

        invTracker = createComponent<InventoryTracker>();
        invTracker.Init(primaryPanel, secondaryPanel);
    }

    private T createComponent<T>() where T: Component 
    {
        playerObject.AddComponent<T>();
        return playerObject.GetComponent<T>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ExpTracker GetExpTracker() {
        return expTracker;
    }

    public LevelProgress GetLevelProgress() {
        return expTracker.GetLevelProgress();
    }

    public Player GetPlayerInfo() {
        return playerInfo;
    }

}
