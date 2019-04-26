using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : Shootable
{
    public float Damage;
    public float CoolDown;

    private NavMeshAgent enemyAgent;
    private BoxCollider enemyCollider;

    private bool inRange;
    private string targetTag;
    private GameObject objInRange;

    private float attackTimer; 

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        enemyAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<BoxCollider>();
        inRange = false;
        objInRange = null;
        targetTag = "";
        attackTimer = 0f;

        GameManager.AddEnemy();
    }

    void OnCollisionEnter(Collision col)
    {

        Shootable targetHit = col.gameObject.GetComponent<Shootable>();
        if (targetHit != null)
        {
            selectTarget(col.gameObject);
        }
    }

    void OnCollisionExit(Collision col)
    {
        Shootable targetLeft = col.gameObject.GetComponent<Shootable>();
        if (targetLeft != null)
        {
            deselctTarget();
        }
    }


    public override void UpdateBehavior()
    {
        base.UpdateBehavior();
        stepTowardPlayer();
        attack();
        
    }

    public override void DestroyShootable()
    {
        base.DestroyShootable();
        GameManager.RemoveEnemy();
        Destroy(this.gameObject);
    }

    private void attack() {

        attackTimer += Time.deltaTime;

        try
        {
            if (inRange && attackTimer >= CoolDown && inRange)
            {
                attackTimer = 0f;
                Shootable target = objInRange.GetComponent<Shootable>();
                target.TakeDamage(Damage);

                if (!target.IsAlive) {
                    deselctTarget();
                }

            }
        }
        catch(MissingReferenceException) {
            deselctTarget();
        }
    }

    private void stepTowardPlayer() {

        if (!inRange)
        {
            Vector3 curPos = transform.position;

            //Get Player Pos
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Vector3 playerPos = player.transform.position;
                enemyAgent.destination = playerPos;
            }
        }
        
    }

    private void deselctTarget() {
        inRange = false;
        objInRange = null;

        if (targetTag == "Obstacle") {
            resumeMovement();
        }
    }

    private void stopMovement() {
        enemyAgent.isStopped = true;
        enemyAgent.velocity = new Vector3(0,0,0);
    }

    private void resumeMovement() {
        enemyAgent.isStopped = false;
    }

    private void selectTarget(GameObject target) {
        inRange = true;
        objInRange = target.gameObject;
        targetTag = target.tag;

        if (targetTag == "Obstacle")
        {
            stopMovement();
        }

        
    }

  
}
