using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;
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

    /*
     * Settings for a good granade: Rb mass = 3.7 for grande.
     * throwCd = 2;
     * throwUpForce = 12;
     * throwForce = 30;
     */
    [Header("Throw Weapon settings")]
    [SerializeField] private GameObject throwabelObject;
    [SerializeField] private float throwCooldown;
    [SerializeField] private float throwUpForce;
    [SerializeField] private float throwForce = 30;

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

        RangedAttackTreeNode meeleAttackTreeNode =
            new RangedAttackTreeNode(agent, gameObject, playerTransform,
                throwabelObject, throwCooldown, throwUpForce, throwForce);

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