using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Utility.EnemyAI;
using Tree = BehaviorTree.Tree;

public class RangedAI : Tree, IDamagable
{
    private TreeNode topTreeNode;
    private float currentHealth;
    private NavMeshAgent agent;
    [SerializeField] private float startHealth;
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float chasingRange;
    [SerializeField] private Transform[] playerTransform;

    void Start()
    {
        currentHealth = startHealth;
        agent = GetComponent<NavMeshAgent>();
        SetUpTree();
    }

    void Update()
    {
        if (currentHealth < 1)
        {
            gameObject.SetActive(false);
            return;
        }

        topTreeNode.Evaluate();
    }

    protected override TreeNode SetUpTree()
    {
        ChaseTreeNode chaseTreeNode = new ChaseTreeNode(playerTransform, agent);

        RangeTreeNode chasingRangeTreeNode = new RangeTreeNode(chasingRange, playerTransform, transform);

        RangeTreeNode shootingRangeTreeNode = new RangeTreeNode(rangedAttackRange, playerTransform, transform);

        RangedAttackTreeNode meeleAttackTreeNode = new RangedAttackTreeNode(agent, gameObject, playerTransform);

        Sequence chaseSequence = new Sequence(new List<TreeNode> {chasingRangeTreeNode, chaseTreeNode});
        Sequence shootSequence = new Sequence(new List<TreeNode> {shootingRangeTreeNode, meeleAttackTreeNode});

        //Sequence goToCoverSequence = new Sequence(new List<TreeNode> {coverAvaliableNode, goToCoverNode});
        //Selector findCoverSelector = new Selector(new List<TreeNode> {goToCoverSequence, chaseSequence});
        //  Selector tryToTakeCoverSelector = new Selector(new List<TreeNode> {isCoveredTreeNode, findCoverSelector});
        //Sequence mainCoverSequence = new Sequence(new List<TreeNode> {healthTreeNode, tryToTakeCoverSelector});

        //m_TopTreeNode = new Selector(new List<TreeNode> {mainCoverSequence, shootSequence, chaseSequence});
        topTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});


        return topTreeNode;
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
    }
}