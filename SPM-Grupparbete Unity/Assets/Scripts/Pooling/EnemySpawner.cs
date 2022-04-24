using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<MeeleEnemyAI> pool;
    [SerializeField] private MeeleEnemyAI meeleEnemyPrefab;


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


    private void Awake() => pool = new ObjectPool<MeeleEnemyAI>(creatEnamy, OnTakeEnemyAIFromPool, OnReturnBallToPool);

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

    MeeleEnemyAI creatEnamy()
    {
        var enemy = Instantiate(meeleEnemyPrefab, transform.position, quaternion.identity);

        enemy.SetPool(pool);

        return enemy;
    }

    void OnTakeEnemyAIFromPool(MeeleEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.transform.position = SpawnPos;
        meeleEnemyAI._meshRenderer.enabled = true;
        meeleEnemyAI.gameObject.SetActive(true);
    }


    public void OnReturnBallToPool(MeeleEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.gameObject.SetActive(false);
    }
}