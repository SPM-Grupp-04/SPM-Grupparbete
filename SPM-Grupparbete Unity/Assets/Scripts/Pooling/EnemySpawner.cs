using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private IObjectPool<EnemyAI> pool;
   [SerializeField] private EnemyAI enemyPrefab;
    public static int totalAmountOfEnemies = 0;
    private void Awake() => pool = new ObjectPool<EnemyAI>(creatEnamy,OnTakeEnemyAIFromPool, OnReturnBallToPool);


    private void Update()
    {
        for (;totalAmountOfEnemies < 10; totalAmountOfEnemies++)
        {
            creatEnamy();
            OnTakeEnemyAIFromPool(enemyPrefab);
        }   
    }
    
    EnemyAI creatEnamy()
    {
        var enemy = Instantiate(enemyPrefab);
        enemy.SetPool(pool);
        return enemy;

    }

    void OnTakeEnemyAIFromPool(EnemyAI enemyAI)
    {
        enemyAI.gameObject.SetActive(true);
        
    }


    void OnReturnBallToPool(EnemyAI enemyAi)
    {
        enemyAi.gameObject.SetActive(false);
    }
   
    private void Start()
    {
        
    }

    
}
