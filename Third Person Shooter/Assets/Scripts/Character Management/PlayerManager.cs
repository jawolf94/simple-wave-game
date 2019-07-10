using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int startingSanity;

    public Text experienceText;
    public Text levelText;
    public Text SanityText;
    public Text LightActionText; 

    public GameObject primaryPanel;
    public GameObject secondaryPanel;

    private GameObject playerObject;
    private Player playerInfo;
    private WillPowerTracker expTracker;
    private InventoryTracker invTracker;

    private FireWeapon fireWeapon;


    // Start is called before the first frame update
    void Start()
    {
        //Get Player object
        playerObject = GameObject.FindGameObjectWithTag("Player");

        //Add Player
        playerInfo = createComponent<Player>();
        playerInfo.Init(startingSanity, 0, SanityText);

        //Set up Exp Tracker
        expTracker = createComponent<WillPowerTracker>();
        expTracker.Init(experienceText, levelText);

        //Set Up Inventroy Tracker
        invTracker = createComponent<InventoryTracker>();
        invTracker.Init(this, primaryPanel, secondaryPanel, LightActionText);

        //Get Weapon Controls
        fireWeapon = GetComponent<FireWeapon>();
        ToggleShot(true);
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

    public WillPowerTracker GetExpTracker() {
        return expTracker;
    }

    public Player GetPlayerInfo() {
        return playerInfo;
    }

    public void ToggleShot(bool onOff) {
        fireWeapon.ShotEnabled = onOff;
    }

}
