using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : EgilTree
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

    [SerializeField] private Transform playerTransform;

    private Transform bestCoveSpot;
    [SerializeField] private Cover[] avaliableCovers;
    private NavMeshAgent agent;

    private Material material;

    private EgilNode topNode;


    void Start()
    {
        _currentHealth = startingHealth;
        SetUpTree();
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
    }

    private void Update()
    {
        if (_currentHealth < startingHealth)
        {
            _currentHealth += Time.deltaTime * healthRestoreRate;
        }

        topNode.Evaluate();
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
    }


    protected override EgilNode SetUpTree()
    {
        IsCoverAvaliableNode coverAvaliableNode = new IsCoverAvaliableNode(avaliableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThresseHold);
        IsCoverdNode isCoveredNode = new IsCoverdNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this, playerTransform);

        EgilSequence chaseSequence = new EgilSequence(new List<EgilNode> {chasingRangeNode, chaseNode});
        EgilSequence shootSequence = new EgilSequence(new List<EgilNode> {shootingRangeNode, shootNode});

        EgilSequence goToCoverSequence = new EgilSequence(new List<EgilNode> {coverAvaliableNode, goToCoverNode});
        EgilSelector findCoverSelector = new EgilSelector(new List<EgilNode> {goToCoverSequence, chaseSequence});
        EgilSelector tryToTakeCoverSelector = new EgilSelector(new List<EgilNode> {isCoveredNode, findCoverSelector});
        EgilSequence mainCoverSequence = new EgilSequence(new List<EgilNode> {healthNode, tryToTakeCoverSelector});

        topNode = new EgilSelector(new List<EgilNode> {mainCoverSequence, shootSequence, chaseSequence});
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
}