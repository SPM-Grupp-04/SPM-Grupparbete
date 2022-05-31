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
    private IObjectPool<BaseEnemyAI> pool;
    private BaseEnemyAI enemy;
    private Vector3 SpawnPos;
    private BoxCollider boxCollider;
    private int inActive = 0;
    private float totalProcent;
    //private EnemyAIHandler enemyAIHandler = EnemyAIHandler.Instance;

    private EnemyAIHandler enemyAIHandler;
    [SerializeField] private BaseEnemyAI[] genericListOfBaseClassEnemyAI;
    [SerializeField] private float[] prioListMatchingObjektOrder;
    [SerializeField] private int totalAllowedEnimesAtSpawner = 10;
    [SerializeField] private float totalAllowedSpawnTime = 5;
    private float timer;


    

  

    private void Awake()
    {
        Debug.Log("Awake in EnemySpawner");
        timer = totalAllowedSpawnTime;
        pool = new ObjectPool<BaseEnemyAI>(CreateEnemy, OnTakeEnemyAIFromPool, OnReturnBallToPool);
        Debug.Log("After Creating a new pool");
        enemyAIHandler = GetComponent<EnemyAIHandler>();

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
        Debug.Log("After Calculating how many enemies that should spawn");
        for (var i = 0; i < genericListOfBaseClassEnemyAI.Length; i++) // 2 gånger
        {
            enemy = genericListOfBaseClassEnemyAI[i];
            for (int j = 0; j < prioListMatchingObjektOrder[i] * totalAllowedEnimesAtSpawner; j++) // 50,28,22
            {
                CreateEnemy();
            }
        }
        Debug.Log("After creating enemies. End of Awake.");
    }

    private void Start()
    {
        Debug.Log("In start set all enemies to Inactive");
        foreach (var enemyAI in enemyAIHandler.units)
        {
            enemyAI.gameObject.SetActive(false);
        }
        Debug.Log("End of start.");
    }
    
    private void OnEnable()
    {
        Debug.Log("Entering Enable");
        foreach (var enemyAI in enemyAIHandler.units)
        {
            enemyAI.gameObject.SetActive(true);
        }
        timer = totalAllowedSpawnTime;
        Debug.Log("Exiting enable");
    }

    
    private void FixedUpdate()
    {
        
        if (inActive < totalAllowedEnimesAtSpawner && timer > 0)
        {
            Debug.Log("Setting spawn position ");
            SpawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);
            for (var i = 0; i < pool.CountInactive; i++)
            {
                Debug.Log("Removing objekts from the pool");
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
        inActive++;
        enemy.SetPool(pool);
        
        Debug.Log("Should have spawned Enemies");
        return enemy;
    }

   private void OnTakeEnemyAIFromPool(BaseEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.transform.position = SpawnPos;
        meeleEnemyAI.gameObject.SetActive(true);
        inActive--;
        Debug.Log("Takeing the eneimes from the pool.");
    }


    public void OnReturnBallToPool(BaseEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.gameObject.SetActive(false);
        inActive++;
        Debug.Log("Returning enemies to the pool");
    }
}