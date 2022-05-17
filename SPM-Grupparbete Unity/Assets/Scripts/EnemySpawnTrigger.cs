using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawner;
    //[SerializeField] private AudioManager audioManager;
    

    private void Start()
    {
        enemySpawner.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            enemySpawner.SetActive(true);
            /*
            if (audioManager.InCombat() == false) 
            {
                audioManager.CombatMusic();
            }
            */
        }
    }
}
