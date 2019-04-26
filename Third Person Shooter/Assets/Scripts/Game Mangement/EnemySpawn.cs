using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public float SpawnTime;
    public GameObject Enemy;
    public bool IsEnabled { get; private set; }

    private float timer;
    

    void Start()
    {
        timer = 0f;
        IsEnabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (IsEnabled) {
            Produce();
        }

    }

    public void ToggleEnabled() {
        IsEnabled = !IsEnabled;
    }

    public void ResetSpawn()
    {
        timer = 0.0f; 
    }

    private void Produce()
    {
        if (timer >= SpawnTime && IsEnabled)
        {
            timer = 0f;
            Instantiate(Enemy, transform);
        }
    }
}

