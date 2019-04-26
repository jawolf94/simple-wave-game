using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Shootable
{
    public Text HealthText { get; set; }
    private string healthTextString = "Health: {0}";

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        GameManager.AddPlayer();
    }

    public void Init(int health, int expReward, Text healthText)
    {
        StartingHealth = health;
        ExperienceReward = expReward;
        HealthText = healthText;
        updateHealthText(StartingHealth); 
    }

    public override void UpdateBehavior()
    {
        base.UpdateBehavior();

    }

    public override void DestroyShootable()
    {
        base.DestroyShootable();
        GameManager.RemovePlayer();
        Destroy(this.gameObject);
    }

    public override void TakeDamage(float damageTaken)
    {
        base.TakeDamage(damageTaken);
        updateHealthText(Health);
    }

    private void updateHealthText(float value)
    {
        HealthText.text = string.Format(
            healthTextString,
            value);
    }





}
