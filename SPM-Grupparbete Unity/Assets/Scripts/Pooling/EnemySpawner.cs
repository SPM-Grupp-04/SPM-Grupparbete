using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<EnemyAI> pool;
    [SerializeField] private EnemyAI enemyPrefab;

   private  int totalAmountOfEnemies = 0;

   [SerializeField] private int totalAllowedEnimesAtSpawner = 10;
    //private  int totalSpawnedEnemies = 0;
    private void Awake() => pool = new ObjectPool<EnemyAI>(creatEnamy, OnTakeEnemyAIFromPool, OnReturnBallToPool);


  

    private void Update()
    {
        /*if (totalSpawnedEnemies < 10)
        {
            creatEnamy();
            Debug.Log(totalAmountOfEnemies + " Total amout of enimes.");
            totalSpawnedEnemies++;
        }*/

        // Spawna finenderna från poolen!

        if (totalAmountOfEnemies < totalAllowedEnimesAtSpawner)
        {
            pool.Get();
        }
    }

    EnemyAI creatEnamy()
    {
        var enemy = Instantiate(enemyPrefab, transform.position, quaternion.identity);
        enemy.SetPool(pool);

        return enemy;
    }

    void OnTakeEnemyAIFromPool(EnemyAI enemyAI)
    {
        enemyAI.transform.position = gameObject.transform.position;
        enemyAI._meshRenderer.enabled = true;
        enemyAI.gameObject.SetActive(true);
        
        totalAmountOfEnemies++;
    }


    public void OnReturnBallToPool(EnemyAI enemyAi)
    {
        enemyAi.gameObject.SetActive(false);
        totalAmountOfEnemies--;
    }
}