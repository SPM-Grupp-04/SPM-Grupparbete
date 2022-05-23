using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<BaseClassEnemyAI> pool;
    private BaseClassEnemyAI _baseClassEnemy;
    private Vector3 SpawnPos;
    private BoxCollider boxCollider;

    private float totalProcent;
    //private EnemyAIHandler enemyAIHandler = EnemyAIHandler.Instance;

    private EnemyAIHandler enemyAIHandler;
    [SerializeField] private BaseClassEnemyAI[] genericListOfBaseClassEnemyAI;
    [SerializeField] private float[] prioListMatchingObjektOrder;
    [SerializeField] private int totalAllowedEnimesAtSpawner = 10;
    [SerializeField] private float totalAllowedSpawnTime = 5;
    private float timer;

    private void Start()
    {
    }

    private void OnEnable()
    {
        timer = totalAllowedSpawnTime;
    }

    private void Awake()
    {
        timer = totalAllowedSpawnTime;
        pool = new ObjectPool<BaseClassEnemyAI>(CreateEnemy, OnTakeEnemyAIFromPool, OnReturnBallToPool);

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
            _baseClassEnemy = genericListOfBaseClassEnemyAI[i];
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


    private BaseClassEnemyAI CreateEnemy()
    {
        // så att man kan sätta hur många % av en typ man vill ska finnas.
        if (_baseClassEnemy == null)
            return null;

        _baseClassEnemy = Instantiate(_baseClassEnemy, transform.position, quaternion.identity);
        enemyAIHandler.units.Add(_baseClassEnemy);

        _baseClassEnemy.SetPool(pool);

        return _baseClassEnemy;
    }

   private void OnTakeEnemyAIFromPool(BaseClassEnemyAI meeleBaseClassEnemyAI)
    {
        meeleBaseClassEnemyAI.transform.position = SpawnPos;
        meeleBaseClassEnemyAI.gameObject.SetActive(true);
    }


    public void OnReturnBallToPool(BaseClassEnemyAI meeleBaseClassEnemyAI)
    {
        meeleBaseClassEnemyAI.gameObject.SetActive(false);
    }
}