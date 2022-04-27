using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private  ObjectPool<BaseClassEnemyAI> pool;
    [SerializeField] private BaseClassEnemyAI[] genericListOfBaseClassEnemyAI;
    [SerializeField] private float[] prioListMatchingObjektOrder;

    // [SerializeField] private BaseClassEnemyAI meleeAIEnemy;
    // [SerializeField] private BaseClassEnemyAI rangedAIEnemy;
    [SerializeField] private int totalAllowedEnimesAtSpawner = 10;


    private Vector3 SpawnPos;
    private BoxCollider boxCollider;


    private float totalProcent;

    private void Start()
    {
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
            for (int j = 0; j < prioListMatchingObjektOrder[i] * totalAllowedEnimesAtSpawner; j++) // 50,28,22
            {
                enemy = genericListOfBaseClassEnemyAI[i];
                creatEnamy();
            }
        }
    }

    private BaseClassEnemyAI enemy;

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
            gameObject.SetActive(false);
        }

        timer -= Time.deltaTime;
    }


    BaseClassEnemyAI creatEnamy()
    {
        // så att man kan sätta hur många % av en typ man vill ska finnas.

        Instantiate(enemy, transform.position, quaternion.identity);

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