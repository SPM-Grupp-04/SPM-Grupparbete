using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Tree = BehaviorTree.Tree;

public class EnemyAI : Tree, IDamagable
{
    [SerializeField] private float startingHealth;

    private float currentHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    [SerializeField] private float lowHealthThreseHold;
    [SerializeField] private float healthRestoreRate;
    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform[] playerTransform;

    // private Transform bestCoveSpot;
    //[SerializeField] private Cover[] avaliableCovers;
    private NavMeshAgent agent;
    private Material material;
    private TreeNode m_TopTreeNode;


    void Start()
    {
        base.Start();
        timeRemaining = cooldownTime;
        currentHealth = startingHealth;
        SetUpTree();
    }


    private void Update()
    {
        if (currentHealth < startingHealth)
        {
            currentHealth += Time.deltaTime * healthRestoreRate;
        }

        if (currentHealth <= 1)
        {
            var die = new DieEvenInfo(gameObject);

            EventSystem.current.FireEvent(die);
            return;
        }

        m_TopTreeNode.Evaluate();
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

    protected override TreeNode SetUpTree()
    {
        //IsCoverAvaliableTreeNode coverAvaliableNode = new IsCoverAvaliableTreeNode(avaliableCovers, playerTransform, this);
        // GoToCoverTreeNode goToCoverNode = new GoToCoverTreeNode(agent, this);
        HealthTreeNode healthTreeNode = new HealthTreeNode(this, lowHealthThreseHold);
        IsCoverdTreeNode isCoveredTreeNode = new IsCoverdTreeNode(playerTransform, transform);
        ChaseTreeNode chaseTreeNode = new ChaseTreeNode(playerTransform, agent, this);

        RangeTreeNode chasingRangeTreeNode = new RangeTreeNode(chasingRange, playerTransform, transform);

        RangeTreeNode shootingRangeTreeNode = new RangeTreeNode(shootingRange, playerTransform, transform);

        ShootTreeNode shootTreeNode = new ShootTreeNode(agent, this, playerTransform);
        Sequence chaseSequence = new Sequence(new List<TreeNode> {chasingRangeTreeNode, chaseTreeNode});
        Sequence shootSequence = new Sequence(new List<TreeNode> {shootingRangeTreeNode, shootTreeNode});

        //Sequence goToCoverSequence = new Sequence(new List<TreeNode> {coverAvaliableNode, goToCoverNode});
        //Selector findCoverSelector = new Selector(new List<TreeNode> {goToCoverSequence, chaseSequence});
        //  Selector tryToTakeCoverSelector = new Selector(new List<TreeNode> {isCoveredTreeNode, findCoverSelector});
        //Sequence mainCoverSequence = new Sequence(new List<TreeNode> {healthTreeNode, tryToTakeCoverSelector});

        //m_TopTreeNode = new Selector(new List<TreeNode> {mainCoverSequence, shootSequence, chaseSequence});
        m_TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});

        return m_TopTreeNode;
    }


    public void SetColor(Color color)
    {
        material.color = color;
    }

    /*public void SetBestCover(Transform bestSpot)
    {
        this.bestCoveSpot = bestSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoveSpot;
    }*/

    public void DealDamage(int damage)
    {
        CurrentHealth -= damage;
    }
}