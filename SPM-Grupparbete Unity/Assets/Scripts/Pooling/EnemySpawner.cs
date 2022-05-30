using System;
using System.Collections;
using System.Collections.Generic;
using EnemyAI;
using EnemyAI.EnemyAIHandler;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<BaseEnemyAI> pool;
    private BaseEnemyAI enemy;
    private Vector3 SpawnPos;
    private BoxCollider boxCollider;

    private float totalProcent;
    //private EnemyAIHandler enemyAIHandler = EnemyAIHandler.Instance;

    private EnemyAIHandler enemyAIHandler;
    [SerializeField] private BaseEnemyAI[] genericListOfBaseClassEnemyAI;
    [SerializeField] private float[] prioListMatchingObjektOrder;
    [SerializeField] private int totalAllowedEnimesAtSpawner = 10;
    [SerializeField] private float totalAllowedSpawnTime = 5;
    private float timer;

    private void Start()
    {
        foreach (var enemyAI in enemyAIHandler.units)
        {
            enemyAI.gameObject.SetActive(false);
        }
    }

    

    private void OnEnable()
    {
        foreach (var enemyAI in enemyAIHandler.units)
        {
            enemyAI.gameObject.SetActive(true);
        }
        timer = totalAllowedSpawnTime;
    }

    private void Awake()
    {
        timer = totalAllowedSpawnTime;
        pool = new ObjectPool<BaseEnemyAI>(CreateEnemy, OnTakeEnemyAIFromPool, OnReturnBallToPool);

        enemyAIHandler = GetComponent<EnemyAIHandler>();
        /*for (int i = 0; i < gameObjects.Length; i++)
        {
            genericListOfBaseClassEnemyAI[i] = gameObjects[i].GetComponent<BaseClassEnemyAI>();
        }*/

       // Räknar ut vad som är 100%
        for (int i = 0; i < genericListOfBaseClassEnemyAI.Length; i++)
        {
            totalProcent += prioListMatchingObjektOrder[i];
        } 

        //Sätter procent av den totala summan prioNummret är.
        for (int i = 0; i < genericListOfBaseClassEnemyAI.Length; i++)
        {
            prioListMatchingObjektOrder[i] /= totalProcent;
        }

        boxCollider = GetComponent<BoxCollider>();
        SpawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);

        for (var i = 0; i < genericListOfBaseClassEnemyAI.Length; i++) // 2 gånger
        {
            enemy = genericListOfBaseClassEnemyAI[i];
            for (int j = 0; j < prioListMatchingObjektOrder[i] * totalAllowedEnimesAtSpawner; j++) // 50,28,22
            {
                CreateEnemy();
            }
        }
    }


    private void FixedUpdate()
    {
        if (pool.CountActive < totalAllowedEnimesAtSpawner && timer > 0)
        {
            SpawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);
            for (var i = 0; i < pool.CountInactive; i++)
            {
                pool.Get();
            }
        }
        else
        {
            this.enabled = false;
        }

        timer -= Time.deltaTime;
    }


    private BaseEnemyAI CreateEnemy()
    {
        // så att man kan sätta hur många % av en typ man vill ska finnas.

        enemy = Instantiate(enemy, transform.position, quaternion.identity);
        enemyAIHandler.units.Add(enemy);

        enemy.SetPool(pool);
        
        return enemy;
    }

   private void OnTakeEnemyAIFromPool(BaseEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.transform.position = SpawnPos;
        meeleEnemyAI.gameObject.SetActive(true);
    }


    public void OnReturnBallToPool(BaseEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.gameObject.SetActive(false);
    }
}