
using EnemyAI;
using EnemyAI.EnemyAIHandler;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<BaseEnemyAI> pool;
    private BaseEnemyAI enemy;
    private Vector3 spawnPos;
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
        /*foreach (var enemyAI in enemyAIHandler.units)
        {
            pool.Release(enemyAI);
        }*/

        this.enabled = false;
    }
    
   
    private void Awake()
    {
        timer = totalAllowedSpawnTime;
        pool = new ObjectPool<BaseEnemyAI>(CreateEnemy, OnTakeEnemyAIFromPool, OnReturnBallToPool);

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
        spawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);

        for (var i = 0; i < genericListOfBaseClassEnemyAI.Length; i++) // 2 gånger
        {
            enemy = genericListOfBaseClassEnemyAI[i];
            for (int j = 0; j < prioListMatchingObjektOrder[i] * totalAllowedEnimesAtSpawner; j++) // 50,28,22
            {
                CreateEnemy();
                pool.Release(enemy);
            }
        }
    }

    private bool spawnEnemies = true;
    private void FixedUpdate()
    {
        if(spawnEnemies == false) 
        {
            return;
        }
        
        
        if (pool.CountActive < totalAllowedEnimesAtSpawner && timer > 0)
        {
            spawnPos = Random.insideUnitSphere + (transform.position * boxCollider.size.x * boxCollider.size.z);
            for (var i = 0; i < pool.CountInactive; i++)
            {
                pool.Get();
            }
        }
        else
        {
            spawnEnemies = false;   
            enabled = false;
            pool = null;
        }
        
        
      
    }

    private void Update()
    {
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

   private void OnTakeEnemyAIFromPool(BaseEnemyAI BaseEnemyAI)
    {
        BaseEnemyAI.transform.position = spawnPos;
        BaseEnemyAI.gameObject.SetActive(true);
      
    }


    public void OnReturnBallToPool(BaseEnemyAI meeleEnemyAI)
    {
        meeleEnemyAI.gameObject.SetActive(false);
    }
}