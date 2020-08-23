using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public controller flags
    public bool inventoryEnabled;
    
    // Public Properties

    /// <summary>
    /// Actions associated with the player's inventory.
    /// </summary>
    public ActionsInventory InventoryActions { get; private set; }

    /// <summary>
    /// Actions associated with Light GameObjects
    /// </summary>
    public ActionsLight LightActions { get; private set; }

    /// <summary>
    /// Actions assoicated with moving the player.
    /// </summary>
    public ActionsMovement MovementActions { get; private set; }

    /// <summary>
    /// Actions associated with the player's weapons.
    /// </summary>
    public ActionsWeapons WeaponActions { get; private set; }

    /// <summary>
    /// Actions associated with the players will power.
    /// </summary>
    public ActionsWillPower WillPowerActions { get; private set; }

    /// <summary>
    /// The game manager for the current scene.
    /// </summary>
    public GameManager GameManager { get; private set; }

    // Private variables


    /// <summary>
    /// Array of referencing all IPlayerAction components attached to the Player Object.
    /// </summary>
    private IPlayerAction[] IPlayerActions; 


    // Key Mapping for Player Controls

    // Inventory Controls
    private readonly string primaryInvScrollDown = "q";
    private readonly string primaryInvScrollUp = "e";
    private readonly string secondaryInvSelect1 = "1";
    private readonly string secondaryInvSelect2 = "2";
    private readonly string secondaryInvSelect3 = "3";
    private readonly string secondaryInvSelect4 = "4";
    private readonly string secondaryInvSelect5 = "5";

    // Light Controls
    private readonly string lightPlayerAction = "Fire2";

    // Movement Controls
    private readonly string movementHorizontal = "Horizontal";
    private readonly string movementVertical = "Vertical";

    // Weapons controls
    private readonly string weaponFirePrimary = "Fire1";


    // Start is called before the first frame update
    void Start()
    {

        // Get array of all assoicated IPlayerActions attached to Player Object
        IPlayerActions = GetComponents<IPlayerAction>();

        // Associate All Player Action Scripts to corresponging property
        InventoryActions = inventoryEnabled ? associateComponent<ActionsInventory>() : null;
 
        LightActions = associateComponent<ActionsLight>();
        MovementActions = associateComponent<ActionsMovement>();
        WeaponActions = associateComponent<ActionsWeapons>();
        WillPowerActions = associateComponent<ActionsWillPower>();

        // Get the scene's game manager
        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

       
    }

    // Update is called once per frame. 
    private void Update()
    {
        // Execute all IPlayerAction PreUpdate functionality
        preUpdate();

        // Check for inventory key mappings
        if (inventoryEnabled)
        {
            // Inventory Primary Item Selection Key Mapping
            if (Input.GetKeyDown(primaryInvScrollDown))
            {
                InventoryActions.ScrollPrimary(false);
            }
            if (Input.GetKeyDown(primaryInvScrollUp))
            {
                InventoryActions.ScrollPrimary(true);
            }

            // Inventory Secondary Item Selection Key Mapping
            if (Input.GetKeyDown(secondaryInvSelect1))
            {
                InventoryActions.SelectSecondary(1);
            }
            if (Input.GetKeyDown(secondaryInvSelect2))
            {
                InventoryActions.SelectSecondary(2);
            }
            if (Input.GetKeyDown(secondaryInvSelect3))
            {
                InventoryActions.SelectSecondary(3);
            }
            if (Input.GetKeyDown(secondaryInvSelect4))
            {
                InventoryActions.SelectSecondary(4);
            }
            if (Input.GetKeyDown(secondaryInvSelect5))
            {
                InventoryActions.SelectSecondary(5);
            }
        }        

        // Light Actions Key Mappings
        if (Input.GetButtonDown(lightPlayerAction))
        {
            if (LightActions.PerformLightAction != null)
            {
                LightActions.PerformLightAction();
            }
        }

        // Player Movement Key Mappings
        MovementActions.MovePosition(Input.GetAxisRaw(movementHorizontal), Input.GetAxisRaw(movementVertical));
        MovementActions.RotatePosition();

        // Player Weapon Actions
        if (Input.GetButton(weaponFirePrimary))
        {
            WeaponActions.Fire();
        }

        // Execute all IPlayerAction PostUpdate functionality
        postUpdate();
    }

    /// <summary>
    /// A function which gets the T compoenet from the GameObject associated with PlayerController. 
    /// The player component's PlayerController references is updated to this object. 
    /// </summary>
    /// <typeparam name="T">An object which inherits the PlayerComponent interface</typeparam>
    /// <returns>T: IPlayerAction - Type specified IPlayerAction compoenent</returns>
    private T associateComponent<T>() where T: Component, IPlayerAction
    {
        T component = GetComponent<T>();
        component.PlayerController = this;
        return component;
    }

    /// <summary>
    /// Function called before checking for Player input. 
    /// Each action IPlayerAction's PreUpdate() will be called. 
    /// </summary>
    private void preUpdate()
    {
        foreach(IPlayerAction action in IPlayerActions)
        {
            action.PreAction();
        }
    }


    /// <summary>
    /// Function called after checking for Player input.
    /// Each IPlayerAction's PostUpdate() will be called. 
    /// </summary>
    private void postUpdate()
    {
        foreach(IPlayerAction action in IPlayerActions)
        {
            action.PostAction();
        }
    }

}
