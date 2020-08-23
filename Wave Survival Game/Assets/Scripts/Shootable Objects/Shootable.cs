using UnityEngine;

public abstract class Shootable : MonoBehaviour
{
    // Vars set in Unity Editor
    public float StartingHealth;

    /// <summary>
    /// Will Power reward is the amount of Will Power gained from killing the object
    /// </summary>
    public int WillPowerReward;

    /// <summary>
    /// The amount of health this object has
    /// </summary>
    public float Health { get; private set; }

    /// <summary>
    /// Bool - True if this object is alive
    /// </summary>
    public bool IsAlive
    {
        get { return Health > 0; }
        private set { }
    }

    /// <summary>
    /// Reference to this scene's game manager
    /// </summary>
    public GameManager GameManager { get; private set; }

    /// <summary>
    /// Bool - True is GameObject can take action in the scene
    /// </summary>
    private bool function;



    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    public void Start()
    {
        // Init Health and funtion to starting values.
        Health = StartingHealth;
        function = true;

        // Find the current GameManager object and get attached script component
        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Perform actions if actions are enabled
        if (function)
        {
            UpdateBehavior();
        }
    }

    /// <summary>
    /// Subtracts the amount of Damage specified from the object total health
    /// </summary>
    /// <param name="damageTaken">The amount of damage to subtract</param>
    public virtual void TakeDamage(float damageTaken)
    {
        // Update total health
        Health -= damageTaken;
        
        // Destroy object if health falls below 0
        if (!IsAlive) {
            DestroyShootable();
        }
    }


    /// <summary>
    /// Action prefromed when Update() is called.
    /// </summary>
    public virtual void UpdateBehavior() {
        return;
    }


    /// <summary>
    /// Action prefomred when Destory() is called.
    /// </summary>
    public virtual void DestroyShootable() {
        function = false;
    }
}
