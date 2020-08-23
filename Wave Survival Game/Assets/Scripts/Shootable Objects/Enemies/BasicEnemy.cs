using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class which defined behavior for a generic enemy unit. It can move, take damage, and attack.
/// </summary>
public class BasicEnemy : Shootable
{
    // Public vars set by Unit Editor

    /// <summary>
    /// The amout of damage delt by this enemy
    /// </summary>
    public float Damage;

    /// <summary>
    /// The amount of time between each attack.
    /// </summary>
    public float CoolDown;

    // Private instance vars

    /// <summary>
    /// Reference to nav mesh on which this enemy navigates
    /// </summary>
    private NavMeshAgent enemyAgent;

    /// <summary>
    /// Collider for this object.
    /// </summary>
    private BoxCollider enemyCollider;

    /// <summary>
    /// True if player is in range
    /// </summary>
    private bool inRange;

    /// <summary>
    /// The tag name of the attackabe object in range.
    /// </summary>
    private string targetTag;

    /// <summary>
    /// Attackable Object current in range
    /// </summary>
    private GameObject objInRange;

    /// <summary>
    /// Timer measuring time passed since last attack was perfomred.
    /// </summary>
    private float attackTimer;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    new void Start()
    {
        // Call shootable Start() function
        base.Start();

        // Get components from current attached GameObject
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<BoxCollider>();

        // Init range vars to reflect that no attackable object is in range.
        inRange = false;
        objInRange = null;
        targetTag = "";
        
        // Set timer to 0
        attackTimer = 0f;

        // Update game manager that a new enemy was created
        GameManager.AddEnemy();
    }

    /// <summary>
    /// Defines action taken when GameObject's collider overlaps with another
    /// </summary>
    /// <param name="col">The other colliding object</param>
    void OnCollisionEnter(Collision col)
    {
        // Check is other object is attackabe (shootable)
        Shootable targetHit = col.gameObject.GetComponent<Shootable>();
        if (targetHit != null)
        {
            // Set target in range if shootable
            selectTarget(col.gameObject);
        }
    }

    /// <summary>
    /// Defiens action taken when GameObject's collider exits the space of another collider
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionExit(Collision col)
    {
        // Check is other object was shootable
        Shootable targetLeft = col.gameObject.GetComponent<Shootable>();
        if (targetLeft != null)
        {
            // Set no target in range
            deselctTarget();
        }
    }

    /// <summary>
    /// Action prefromed when Update() is called.
    /// </summary>
    public override void UpdateBehavior()
    {
        // Preform parent's behavior
        base.UpdateBehavior();

        // Move closer to the player
        stepTowardPlayer();

        // Attack selected in range target
        attack();
        
    }

    /// <summary>
    /// Action prefomred when Destory() is called.
    /// </summary>
    public override void DestroyShootable()
    {
        // Call parent's destory
        base.DestroyShootable();

        // Update game manager to remove enemy
        GameManager.RemoveEnemy();

        // Destroy the game object
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Attacks the target in range 
    /// </summary>
    private void attack() {

        // Add total time elapsed to attackTimer
        attackTimer += Time.deltaTime;

        try
        {
            // Attack if target is in range and enough time has elapsed since last attack
            if (inRange && attackTimer >= CoolDown)
            {
                // Reset timer
                attackTimer = 0f;

                // Apply damage to target object
                Shootable target = objInRange.GetComponent<Shootable>();
                target.TakeDamage(Damage);

                // Deselect the target if it has been killed.
                if (!target.IsAlive) {
                    deselctTarget();
                }

            }
        }
        catch(MissingReferenceException) {
            // Deselect the target if an error occurs
            deselctTarget();
        }
    }

    private void stepTowardPlayer() {

        if (!inRange)
        {
            // Get this GameObject current position
            Vector3 curPos = transform.position;

            // Get the Player's GameObject
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Move if the Player's GameObject exists
            if (player != null)
            {
                Vector3 playerPos = player.transform.position;
                enemyAgent.destination = playerPos;
            }
        }
        
    }

    /// <summary>
    /// Updates instance variables to reflect no active target
    /// </summary>
    private void deselctTarget() {
        // Set instance vars to "no target" state
        inRange = false;
        objInRange = null;

        if (targetTag == "Obstacle") {
            resumeMovement();
        }
    }

    /// <summary>
    /// Stops enemy unit from moving when called.
    /// </summary>
    private void stopMovement() {
        // Stops further movement
        enemyAgent.isStopped = true;

        // Arrests current movement
        enemyAgent.velocity = new Vector3(0,0,0);
    }

    /// <summary>
    /// Continues enemey movement when called
    /// </summary>
    private void resumeMovement() {
        enemyAgent.isStopped = false;
    }

    /// <summary>
    /// Updates instance vars to reflect a selected target.
    /// </summary>
    /// <param name="target">The target to select</param>
    private void selectTarget(GameObject target) {
        inRange = true;
        objInRange = target.gameObject;
        targetTag = target.tag;

        // Stop moving if enemy is attacking an obstacle
        if (targetTag == "Obstacle")
        {
            stopMovement();
        }

        
    }

  
}
