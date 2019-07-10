using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTracker : MonoBehaviour
{
    public Item SelectedPrimary;
    public Item SelectedSeconday;

    private PlayerManager activePlayerManager; 
    private List<Item> primaryWeapons;
    private List<Item> secondaryItems;

    private List<GameObject> primaryButtons;
    private List<GameObject> secondaryButtons;

    private int primaryIndex;
    private int secondaryIndex;

    private int primarySize;
    private int secondarySize;

    private ColorBlock selectedBlock;

    private bool lightInRange;
    private Text lightActionText;

    private GameObject heldLight;
    private GameObject nearbyLight;

    private bool isInitalized = false;

    //Strings that display when a player can preform an action on a light object
    private static string pickUpText = "[RC] to Pick Up Light";
    private static string dropText = "[RC] to Drop Light";
    private static string plugText = "[RC] to Plug in Light";

    //Delegate and variable mapped to player Right-Click Action
    private delegate void LightAction();
    private LightAction lightAction;

    // Start is called before the first frame update
    void Start()
    {   

    }

    public void Init(PlayerManager callingManager, GameObject primaryPanel, GameObject secondaryPanel, Text lightText)
    {

        this.activePlayerManager = callingManager;

        primaryButtons = new List<GameObject>();
        secondaryButtons = new List<GameObject>();

        foreach (Button b in primaryPanel.GetComponentsInChildren<Button>())
        {
            addButtonToPanel(b, primaryButtons);
        }

        foreach (Button b in secondaryPanel.GetComponentsInChildren<Button>())
        {
            addButtonToPanel(b, secondaryButtons);
        }

        primarySize = primaryButtons.Count;
        secondarySize = secondaryButtons.Count;

        primaryWeapons = new List<Item>();
        secondaryItems = new List<Item>();


        primaryIndex = 0;
        secondaryIndex = 0;

        //Temp Code serving as a placeholder for real items

        Item testPrimary = new Item("Test P1");
        AddItemToPrimary(testPrimary);


        Item testSecondary = new Item("Test S1");
        AddItemToSecondary(testSecondary);

        SelectedPrimary = primaryWeapons[primaryIndex];
        SelectedSeconday = secondaryItems[secondaryIndex];

        selectedBlock = ColorBlock.defaultColorBlock;
        selectedBlock.normalColor = Color.yellow;

        setButtonColorSelected(primaryButtons[primaryIndex].GetComponent<Button>());
        setButtonColorSelected(secondaryButtons[secondaryIndex].GetComponent<Button>());

        this.lightActionText = lightText;
        this.heldLight = null;
        this.nearbyLight = null;
        setLightInRange(false);
        lightAction = null;
        isInitalized = true;
    }

    void Update()
    {

        if (isInitalized)
        {
            //Primary Item Selection
            if (Input.GetKeyDown("q"))
            {
                scrollPrimary(false);
            }
            if (Input.GetKeyDown("e"))
            {
                scrollPrimary(true);
            }

            //Secondary Item Selection
            if (Input.GetKeyDown("1"))
            {
                selectSecondary(1);
            }
            if (Input.GetKeyDown("2"))
            {
                selectSecondary(2);
            }
            if (Input.GetKeyDown("3"))
            {
                selectSecondary(3);
            }
            if (Input.GetKeyDown("4"))
            {
                selectSecondary(4);
            }
            if (Input.GetKeyDown("5"))
            {
                selectSecondary(5);
            }

            //Set player right click option
            setLightAction();

            //Carry out right click option 
            if (Input.GetButtonDown("Fire2") && lightAction != null)
            {
                lightAction();
            }
            moveLight();
        }

    }

    

    public void OnTriggerEnter(Collider other)
    {
        //Bool set to true if player picks up an inventrory item
        bool itemAdded = false;

        if (other.tag == "Primary Item")
        {
            itemAdded = AddItemToPrimary(other.GetComponent<BasicItem>().GetItemInfo());
        }

        if (other.tag == "Secondary Item")
        {
            itemAdded = AddItemToSecondary(other.GetComponent<BasicItem>().GetItemInfo());
        }

        //Remove item from the map if it is picked up by the player. 
        if (itemAdded)
        {
            Destroy(other.gameObject);
        }

        //Set light object in range if player enters radius
        if (other.tag.Equals("Light"))
        {
            setLightInRange(true);
            this.nearbyLight = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Deselct Light object if player leaves radius
        if (other.tag.Equals("Light"))
        {
            setLightInRange(false);
            this.nearbyLight = null; 
        }
    }


    public bool AddItemToPrimary(Item newItem)
    {
        if (primaryWeapons.Count < primarySize)
        {
            int newIndex = addItemToSet(newItem, primaryWeapons);
            primaryButtons[newIndex].GetComponentInChildren<Text>().text = newItem.itemName;
            return true;
        }

        return false;
    }

    public bool AddItemToSecondary(Item newItem)
    {
        if (secondaryItems.Count < secondarySize) {
            int newIndex = addItemToSet(newItem, secondaryItems);
            secondaryButtons[newIndex].GetComponentInChildren<Text>().text = newItem.itemName;
            return true;
        }

        return false;
        

    }

    //Sets flags indicating the player is near a light object

    private void setLightAction()
    {
        //Light is nearby and the player does not currently hold a light
        if (lightInRange && heldLight == null)
        {
            PluggableLight light = nearbyLight.GetComponent<PluggableLight>();

            //Nearby light is in range of an outlet
            if (light.OutletInRange)
            {
                //Light is not already plugged in
                if (!light.PluggedIn)
                {
                    //Plug In Light Action displays
                    lightAction = plugInLight;
                    lightActionText.text = plugText;
                    lightActionText.enabled = true;

                }
                else
                {
                    //Light is already plugged in - no action can be taken
                    lightAction = null;
                    lightActionText.text = string.Empty;
                    lightActionText.enabled = false;
                }
            }
            //Player is near a light not plugged in - can pick up
            else 
            {
                //Pick Up the Light Action displays
                lightAction = pickUpLight;
                lightActionText.text = pickUpText;
                lightActionText.enabled = true;
            }
        }
        //Player is holding a light
        else if (heldLight != null)
        {
            //Drop the light action displays for player
            lightAction = dropLight;
            lightActionText.text = dropText;
            lightActionText.enabled = true;
        }
        //Player is not near a light
        else
        {
            lightActionText.text = string.Empty;
            lightActionText.enabled = false;
            lightAction = null;
        }
    }


    private void setLightInRange(bool onOff)
    {
        lightInRange = onOff;
    }

    //Sets light object triggered by player to player's inventory
    //Light Action Delegate
    private void pickUpLight() {
        this.heldLight = this.nearbyLight;
        activePlayerManager.ToggleShot(false);
    }

    //Drops the light currently in the players inventory
    //Light Action Delegate
    private void dropLight() {
        this.heldLight = null;
        activePlayerManager.ToggleShot(true);
    }

    //Plugs in the light currently in the players inventory
    //Light Action Delegate
    private void plugInLight()
    {
        PluggableLight light = nearbyLight.GetComponent<PluggableLight>();
        light.PlugInLight();
    }

    //Moves light to stay in front of player
    private void moveLight()
    {
        if (this.heldLight != null)
        {
            //Set Initial Light position to the postion of the player
            Vector3 lightPos = this.transform.position;

            //Get the width of the light and half the width of the player
            float lightWidth = this.heldLight.transform.localScale.z/2;
            float playerWidth = this.transform.localScale.z / 2;

            //Multiply the scalar vector of player's position by distance from player's center. 
            //Result = [X,0,0] where X is disantce in forward direction. 
            //Add to players position.
            lightPos += this.transform.forward * (lightWidth + playerWidth);

            //Set new light postion
            this.heldLight.transform.position = lightPos;

            //Rotate Light
            this.heldLight.transform.rotation = this.transform.rotation;
        }
    }

    private void addButtonToPanel(Button b, List<GameObject> panel)
    {
        GameObject buttonObj = b.gameObject;
        buttonObj.GetComponentInChildren<Text>().text = "Empty";
        panel.Add(buttonObj);
    }


    private int addItemToSet(Item newItem, List<Item> set) {
        set.Add(newItem);
        return set.LastIndexOf(newItem);
    }

    private void scrollPrimary(bool up) {
        int priorIndex = primaryIndex;

        if (primaryWeapons.Count <= 1) {
            return;
        }

        if (up)
        {
            primaryIndex++;
            if (primaryIndex >= primaryWeapons.Count) {
                primaryIndex = 0;
            }
        }

        else {
            primaryIndex--;
            if (primaryIndex < 0) {
                primaryIndex = primaryWeapons.Count - 1;
            }
        }

        SelectedPrimary = primaryWeapons[primaryIndex];

        setButtonColorSelected(primaryButtons[primaryIndex].GetComponent<Button>());
        setButtonColorDeselected(primaryButtons[priorIndex].GetComponent<Button>());

    }

    private void selectSecondary(int selection) {
 
        if (secondaryItems.Count >= selection && selection-1 != secondaryIndex) {
            int previousSelection = secondaryIndex;
            secondaryIndex = selection - 1;
            SelectedSeconday = secondaryItems[secondaryIndex];

            //Button Logic
            setButtonColorSelected(secondaryButtons[secondaryIndex].GetComponent<Button>());
            setButtonColorDeselected(secondaryButtons[previousSelection].GetComponent<Button>());
           
        }
        else if(secondaryItems.Count < selection){
            Debug.Log("No Item in slot");
        }   

        

    }


    private void setButtonColorSelected(Button button) {
        button.colors = selectedBlock;
    }

    private void setButtonColorDeselected(Button button) {
        button.colors = ColorBlock.defaultColorBlock;
    }

    


}
