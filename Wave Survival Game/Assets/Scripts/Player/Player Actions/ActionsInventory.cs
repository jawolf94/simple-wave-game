using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to manage a player's inventory based on the player's actions.
/// Note: This functionality is currently disabled.
/// </summary>
public class ActionsInventory : MonoBehaviour, IPlayerAction
{
    // Unity UI set public vars

    /// <summary>
    /// The item panel in the UI used to display a player's primary weapons.
    /// </summary>
    public GameObject PrimaryPanel;

    /// <summary>
    /// The item panel in the UI used to display a player's secondary items.
    /// </summary>
    public GameObject SecondaryPanel;

    // Public Properties

    /// <summary>
    /// The Player controller referencing this object. 
    /// </summary>
    public PlayerController PlayerController { get; set; }

    /// <summary>
    /// The Primary Item object currently selected by the player.
    /// </summary>
    public Item SelectedPrimary { get; private set; }

    /// <summary>
    /// The Secondry Item object currently selected by the player.
    /// </summary>
    public Item SelectedSeconday { get; private set; }

    // Private variables

    /// <summary>
    /// List of Items representing the Player's primary weapon inventory.
    /// </summary>
    private List<Item> primaryWeapons;

    /// <summary>
    /// List of Items representign the Player's secondary item inventory.
    /// </summary>
    private List<Item> secondaryItems;

    /// <summary>
    /// List of GameObject's referencing the UI Buttons displaying primaryWeapons.
    /// </summary>
    private List<GameObject> primaryButtons;

    /// <summary>
    /// List of GameObjects referencing the UI Buttons displaying secondaryItems.
    /// </summary>
    private List<GameObject> secondaryButtons;

    /// <summary>
    /// Index of currently selected primaryItem.
    /// </summary>
    private int primaryIndex;

    /// <summary>
    /// Index of currently selected secondaryItem.
    /// </summary>
    private int secondaryIndex;

    /// <summary>
    /// Total number of Primary Items a Player can hold.
    /// </summary>
    private int primarySize;

    /// <summary>
    /// Total number of Secondary Items a Player can hold.
    /// </summary>
    private int secondarySize;

    /// <summary>
    /// ColorBlock used to highlight the currently selected item.
    /// </summary>
    private ColorBlock selectedBlock;



    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {

        // Instantiate lists to keep track of the UI buttons representing the player's inventory
        primaryButtons = new List<GameObject>();
        secondaryButtons = new List<GameObject>();

        // Get primary weapon buttons from Primary Panel on UI
        foreach (Button b in PrimaryPanel.GetComponentsInChildren<Button>())
        {
            addButtonToPanel(b, primaryButtons);
        }

        // Get secondary weapon buttons from Secondary Panel on UI
        foreach (Button b in SecondaryPanel.GetComponentsInChildren<Button>())
        {
            addButtonToPanel(b, secondaryButtons);
        }

        // Set the size of each item inventory
        primarySize = primaryButtons.Count;
        secondarySize = secondaryButtons.Count;

        // Instantiate list of items track each UI item
        primaryWeapons = new List<Item>();
        secondaryItems = new List<Item>();

        // Starting index for each selected item. 
        primaryIndex = 0;
        secondaryIndex = 0;

        // Temp Code serving as a placeholder for real items

        Item testPrimary = new Item("Test P1");
        AddItemToPrimary(testPrimary);


        Item testSecondary = new Item("Test S1");
        AddItemToSecondary(testSecondary);

        // Store colors to use when items are selected/de-selected.
        selectedBlock = ColorBlock.defaultColorBlock;
        selectedBlock.disabledColor = Color.yellow;

        // Sets currently selected items
        SelectedPrimary = primaryWeapons[primaryIndex];
        SelectedSeconday = secondaryItems[secondaryIndex];

        setButtonColorSelected(primaryButtons[primaryIndex].GetComponent<Button>());
        setButtonColorSelected(secondaryButtons[secondaryIndex].GetComponent<Button>());
    }

    /// <summary>
    /// Called when the player enters a Trigger Collider
    /// </summary>
    /// <param name="other">The other object's collirder</param>
    public void OnTriggerEnter(Collider other)
    {
        // Bool set to true if player picks up an inventrory item
        bool itemAdded = false;

        // Actions if player steps near a Primary Item
        if (other.tag == "Primary Item")
        {
            itemAdded = AddItemToPrimary(other.GetComponent<BasicItem>().GetItemInfo());
        }

        // Actions if player steps near a Secondary Item.
        if (other.tag == "Secondary Item")
        {
            itemAdded = AddItemToSecondary(other.GetComponent<BasicItem>().GetItemInfo());
        }

        // Remove item from the map if it is picked up by the player. 
        if (itemAdded)
        {
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Executes before the PlayerController checks for Player Input.
    /// All actions that must be done before the player performs an action should be placed here. 
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
    /// Adds a Primary Item to the Player's inventory.
    /// </summary>
    /// <param name="newItem">Item - Item to be inserted into the Player's primary item inventory.</param>
    /// <returns>Bool - True if item was added successfully.</returns>
    public bool AddItemToPrimary(Item newItem)
    {
        // Check if the Player's inventory is full
        if (primaryWeapons.Count < primarySize)
        {
            // Inventory not full, add the item and update the corresposing UI element.
            int newIndex = addItemToSet(newItem, primaryWeapons);
            primaryButtons[newIndex].GetComponentInChildren<Text>().text = newItem.itemName;
            return true;
        }

        // Inventory full
        return false;
    }

    /// <summary>
    /// Adds a Secondary Item to the Player's inventory.
    /// </summary>
    /// <param name="newItem">Item - Item to be inserted into the Player's secondary item inventory.</param>
    /// <returns>Bool - True if item was added successfully.</returns>
    public bool AddItemToSecondary(Item newItem)
    {
        // Check if the Player's secondary inventory is full.
        if (secondaryItems.Count < secondarySize) {

            // Secondary Inventory not full, add the item and update corresponding UI element.
            int newIndex = addItemToSet(newItem, secondaryItems);
            secondaryButtons[newIndex].GetComponentInChildren<Text>().text = newItem.itemName;
            return true;
        }

        //Inventory full.
        return false;
        

    }

    /// <summary>
    /// Function called to select new primary item.
    /// </summary>
    /// <param name="up">Bool - Selects a higher index if true</param>
    public void ScrollPrimary(bool up)
    {
        // Set previos index to current index.
        int priorIndex = primaryIndex;

        // Stop if the player has 0 or 1 items in their primary inventory.
        if (primaryWeapons.Count <= 1)
        {
            return;
        }

        // If the player scrolled up
        if (up)
        {

            // Increase the primary index by one.
            primaryIndex++;

            // Set the primary index to 0 if the player scrolled up past the last index. 
            if (primaryIndex >= primaryWeapons.Count)
            {
                primaryIndex = 0;
            }
        }
        // Player scrolled down.
        else
        {
            // Decrease the primaryIndex
            primaryIndex--;

            // Set the selected index to the last item in the inventory if the player scrolled past the first index.
            if (primaryIndex < 0)
            {
                primaryIndex = primaryWeapons.Count - 1;
            }
        }

        // Update the selected item using new index.
        SelectedPrimary = primaryWeapons[primaryIndex];

        // Change UI Button colors based on selctions
        setButtonColorSelected(primaryButtons[primaryIndex].GetComponent<Button>());
        setButtonColorDeselected(primaryButtons[priorIndex].GetComponent<Button>());

    }

    /// <summary>
    /// Changes the selected secondary index.
    /// </summary>
    /// <param name="selection">Int - Button number to select</param>
    public void SelectSecondary(int selection)
    {

        // If the index is less than or equal to the number of items and not the current item
        if (secondaryItems.Count >= selection && selection - 1 != secondaryIndex)
        {
            // Current selection becomes previous selection
            int previousSelection = secondaryIndex;

            // Index is updated to button number minus 1
            secondaryIndex = selection - 1;

            // Set the new selected secondary item
            SelectedSeconday = secondaryItems[secondaryIndex];

            // Change UI Button colors based on selction
            setButtonColorSelected(secondaryButtons[secondaryIndex].GetComponent<Button>());
            setButtonColorDeselected(secondaryButtons[previousSelection].GetComponent<Button>());

        }
    }


    /// <summary>
    /// Adds a UI Button to a panel list.
    /// The Button's text will be initializerd to null.
    /// </summary>
    /// <param name="b">Button - The UI Button to add</param>
    /// <param name="panel">List - The set of GameObjects (Primary/Secondary) to which the button will be added</param>
    private void addButtonToPanel(Button b, List<GameObject> panel)
    {
        GameObject buttonObj = b.gameObject;
        buttonObj.GetComponentInChildren<Text>().text = "Empty";
        panel.Add(buttonObj);
    }


    /// <summary>
    /// Adds an item to a list of items
    /// </summary>
    /// <param name="newItem">Item - The Item to add</param>
    /// <param name="set">List - Set of Items (primary or secondary) to recieve the item</param>
    /// <returns></returns>
    private int addItemToSet(Item newItem, List<Item> set) {
        set.Add(newItem);
        return set.LastIndexOf(newItem);
    }

    /// <summary>
    /// Sets a buttons color to the selectedBlock color.
    /// </summary>
    /// <param name="button">Button - The selected Button</param>
    private void setButtonColorSelected(Button button) {
        button.colors = selectedBlock;
    }

    /// <summary>
    /// Sets a button's color to the default color.
    /// </summary>
    /// <param name="button">Button - The deslected button</param>
    private void setButtonColorDeselected(Button button) {

        // Set unselected block disabled color to normal color
        ColorBlock unselectedBlock = ColorBlock.defaultColorBlock;
        unselectedBlock.disabledColor = unselectedBlock.normalColor;

        // Set color block
        button.colors = unselectedBlock;
    }

    


}
