using UnityEngine;

public abstract class Shootable : MonoBehaviour
{
    //Inspector Elements
    public float StartingHealth;

    /// <summary>
    /// Will Power reward is the amount of Will Power gained from killing the object
    /// </summary>
    public int WillPowerReward;
    public float Health { get; private set; }
    public bool IsAlive
    {
        get { return Health > 0; }
        private set { }
    }

    public GameManager GameManager { get; private set; }

    private bool function;



    // Start is called before the first frame update
    public void Start()
    {
        Health = StartingHealth;
        function = true;

        GameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (function)
        {
            UpdateBehavior();
        }
    }

    public virtual void TakeDamage(float damageTaken)
    {
        Health -= damageTaken;
        
        if (!IsAlive) {
            DestroyShootable();
        }
    }

    public virtual void UpdateBehavior() {
        return;
    }

    public virtual void DestroyShootable() {
        function = false;
    }
}
