using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    //Public Settings
    public float CoolDown;
    public float ShotRange;
    public float ShotDamage;

    //True is player can fire primary weapon
    public bool ShotEnabled { get; set; }

    //Private vars for displaying shots    
    private LineRenderer shotLine;
    private int shootableMask;
    private float shotDisplayTime = 0.3f;

    private PlayerManager associatedPlayer;

    //Timer used to track cool down.
    private float timer;

    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        shootableMask = LayerMask.GetMask("Shootable");

        shotLine = GetComponent<LineRenderer>();
        if (shotLine == null) {
            Debug.Log("[FireWeapon] Error: No Line Renderer attached to player object.");
        }

        associatedPlayer = GetComponent<PlayerManager>();
    }
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (ShotEnabled) {
            if (Input.GetButton("Fire1") && timer >= CoolDown)
            {
                fire();
            }
        }

        if (timer >= CoolDown * shotDisplayTime)
        {
            stopEffects();
        }


    }

    private void stopEffects() {
       shotLine.enabled = false; 
    }

    private void fire() {

        timer = 0;

        shotLine.enabled = true;
        shotLine.SetPosition(0, transform.position);

        Ray shotRay = new Ray();
        shotRay.origin = transform.position;
        shotRay.direction = transform.forward;

        RaycastHit shotHit;

        if (Physics.Raycast(shotRay, out shotHit, ShotRange, shootableMask))
        {
            //Hit
            shotLine.SetPosition(1, shotHit.point);

            Shootable targetHit = shotHit.collider.GetComponent<Shootable>();
            targetHit.TakeDamage(ShotDamage);
            if (!targetHit.IsAlive) {
                associatedPlayer.GetExpTracker().ModifyExperience(targetHit.ExperienceReward);
            }
        }
        else
        {
            shotLine.SetPosition(1, shotRay.origin + shotRay.direction * ShotRange);
        }

    }
}
