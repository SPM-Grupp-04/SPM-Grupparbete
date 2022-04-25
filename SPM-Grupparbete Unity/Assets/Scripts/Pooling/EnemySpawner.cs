using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<BaseClassEnemyAI> pool;
    [SerializeField] private BaseClassEnemyAI[] genericListOfBaseClassEnemyAI;


    [SerializeField] private int totalAllowedEnimesAtSpawner = 10;


    private Vector3 SpawnPos;
    private BoxCollider boxCollider;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        SpawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);

        if (pool.CountActive < totalAllowedEnimesAtSpawner)
        {
            SpawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);
            for (var i = 0; i < totalAllowedEnimesAtSpawner; i++)
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
            gameObject.SetActive(false);
        }


        timer -= Time.deltaTime;
    }

    BaseClassEnemyAI enemy = null;
    BaseClassEnemyAI creatEnamy()
    {
      // så att man kan sätta hur många % av en typ man vill ska finnas.
        
        foreach (var prefab in genericListOfBaseClassEnemyAI)
        {
            if (enemy != prefab)
            {
                
                enemy = prefab;
                break;
            }
           
            enemy = prefab;
            
        }

        if (enemy == null)
        {
            return null;
        }
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