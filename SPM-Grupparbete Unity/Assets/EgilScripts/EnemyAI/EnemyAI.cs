using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : EgilTree, IDamagable
{
    [SerializeField] private float startingHealth;

    public float _currentHealth;

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    [SerializeField] private float lowHealthThresseHold;
    [SerializeField] private float healthRestoreRate;
    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform[] playerTransform;

    private Transform bestCoveSpot;
    [SerializeField] private Cover[] avaliableCovers;
    private NavMeshAgent agent;

    private Material material;

    private EgilNode topNode;


    void Start()
    {
        base.Start();
        timeRemaining = cooldownTime;
        _currentHealth = startingHealth;
        SetUpTree();
    }


    private void Update()
    {
        if (_currentHealth < startingHealth)
        {
            _currentHealth += Time.deltaTime * healthRestoreRate;
        }

        if (_currentHealth <= 1)
        {
            var die = new DieEvenInfo(gameObject);
            
            EventSystem.current.FireEvent(die);
            return;
        }

        topNode.Evaluate();
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
    }

    private float cooldownTime = 2f; // Varför måste den vara i hela sekunder.
    private float timeRemaining;

    private void OnCollisionStay(Collision collision)
    {
        if (timeRemaining < 0.0f)
        {
            var damageEvent = new DealDamageEventInfo(collision.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);
        }

        if (timeRemaining < 0.0f)
        {
            timeRemaining = cooldownTime;
        }

        timeRemaining -= Time.deltaTime;
    }

    protected override EgilNode SetUpTree()
    {
        //IsCoverAvaliableNode coverAvaliableNode = new IsCoverAvaliableNode(avaliableCovers, playerTransform, this);
        // GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThresseHold);
        IsCoverdNode isCoveredNode = new IsCoverdNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);

        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);

        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);

        ShootNode shootNode = new ShootNode(agent, this, playerTransform);
        EgilSequence chaseSequence = new EgilSequence(new List<EgilNode> {chasingRangeNode, chaseNode});
        EgilSequence shootSequence = new EgilSequence(new List<EgilNode> {shootingRangeNode, shootNode});

        //EgilSequence goToCoverSequence = new EgilSequence(new List<EgilNode> {coverAvaliableNode, goToCoverNode});
        //EgilSelector findCoverSelector = new EgilSelector(new List<EgilNode> {goToCoverSequence, chaseSequence});
        //  EgilSelector tryToTakeCoverSelector = new EgilSelector(new List<EgilNode> {isCoveredNode, findCoverSelector});
        //EgilSequence mainCoverSequence = new EgilSequence(new List<EgilNode> {healthNode, tryToTakeCoverSelector});

        //topNode = new EgilSelector(new List<EgilNode> {mainCoverSequence, shootSequence, chaseSequence});
        topNode = new EgilSelector(new List<EgilNode> {shootSequence, chaseSequence});

        return topNode;
    }


    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCover(Transform bestSpot)
    {
        this.bestCoveSpot = bestSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoveSpot;
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
    }
}