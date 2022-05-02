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
    private BaseClassEnemyAI enemy;
    private Vector3 SpawnPos;
    private BoxCollider boxCollider;
    private float totalProcent;
    //private EnemyAIHandler enemyAIHandler = EnemyAIHandler.Instance;


    [SerializeField] private BaseClassEnemyAI[] genericListOfBaseClassEnemyAI;
    [SerializeField] private float[] prioListMatchingObjektOrder;
    [SerializeField] private int totalAllowedEnimesAtSpawner = 10;
    
    private void Start()
    {
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
                creatEnamy();
           
            }
        }
    }
    
    private void Awake() =>
        pool = new ObjectPool<BaseClassEnemyAI>(creatEnamy, OnTakeEnemyAIFromPool, OnReturnBallToPool);

    [SerializeField] private float timer = 5;

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
            Debug.Log("Varför bara varöfr?!");
            gameObject.SetActive(false);
        }

        timer -= Time.deltaTime;
    }

    
    BaseClassEnemyAI creatEnamy()
        {
            // så att man kan sätta hur många % av en typ man vill ska finnas.

            enemy = Instantiate(enemy, transform.position, quaternion.identity);

            Debug.Log(" EnemySpawner Pool " + pool); // Inte null.
            enemy.SetPool(pool);

            return enemy;
        }

        void OnTakeEnemyAIFromPool(BaseClassEnemyAI meeleEnemyAI)
        {
            meeleEnemyAI.transform.position = SpawnPos;
            meeleEnemyAI.gameObject.SetActive(true);
        }


        public void OnReturnBallToPool(BaseClassEnemyAI meeleEnemyAI)
        {
            meeleEnemyAI.gameObject.SetActive(false);
        }
    }