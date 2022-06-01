using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawner;
    //[SerializeField] private AudioManager audioManager;

    private EnemySpawner espawner;
    private void Start()
    {
        enemySpawner.SetActive(false);
        espawner = enemySpawner.GetComponent<EnemySpawner>();
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            enemySpawner.SetActive(true);
            espawner.enabled = true;
            /*
            if (audioManager.InCombat() == false) 
            {
                audioManager.CombatMusic();
            }
            */
        }
    }
}
