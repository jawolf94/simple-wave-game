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

    private static string pickUpText = "[RC] to Pick Up Light";
    private static string dropText = "[RC] to Drop Light";

    private bool isInitalized = false; 

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
        this.lightActionText.text = pickUpText;
        setLightInRange(false);

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

            //Light PickUp & Drop
            if (Input.GetButtonDown("Fire2"))
            {
                //Light is held and can be dropped
                if (heldLight != null)
                {
                    dropLight();
                }
                //Light is nearby but not held
                else if(lightInRange)
                {
                    pickUpLight();

                }
                
            }

            //Light Movement
            moveLight();
        }

    }

    

    public void OnTriggerEnter(Collider other)
    {

        bool itemAdded = false;

        if (other.tag == "Primary Item")
        {
            itemAdded = AddItemToPrimary(other.GetComponent<BasicItem>().GetItemInfo());
        }

        if (other.tag == "Secondary Item")
        {
            itemAdded = AddItemToSecondary(other.GetComponent<BasicItem>().GetItemInfo());
        }

        if (itemAdded)
        {
            Destroy(other.gameObject);
        }

        if (other.tag.Equals("Light"))
        {
            setLightInRange(true);
            this.nearbyLight = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
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
    private void setLightInRange(bool onOff)
    {
        lightActionText.enabled = onOff;
        lightInRange = onOff;
    }

    //Sets light obje9ct triggered by player to player's inventory
    private void pickUpLight() {
        this.heldLight = this.nearbyLight;
        this.lightActionText.text = dropText;
        activePlayerManager.ToggleShot(false);
    }

    //Drops the light currently in the players inventory
    private void dropLight() {
        this.heldLight = null;
        activePlayerManager.ToggleShot(true);
        this.lightActionText.text = pickUpText;
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
