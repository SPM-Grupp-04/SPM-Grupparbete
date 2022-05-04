using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Utility.EnemyAI;
using Tree = BehaviorTree.Tree;

public class RangedAI : BaseClassEnemyAI, IDamagable
{
    //  private TreeNode topTreeNode;
    private float currentHealth;
    private NavMeshAgent agent;
    [SerializeField] private float startHealth;
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float chasingRange;
    private Animator _animator;
    private IObjectPool<BaseClassEnemyAI> pool;

    /*
     * Settings for a good granade: Rb mass = 3.7 for grande.
     * throwCd = 2;
     * throwUpForce = 12;
     * throwForce = 30;
     */
    [Header("Throw Weapon settings")] [SerializeField]
    private GameObject throwabelObject;

    [SerializeField] private float throwCooldown;
    [SerializeField] private float throwUpForce;
    [SerializeField] private float throwForce = 30;

    public Vector3 target = new Vector3(100, 0, 100);
    public float distanceToTargetPlayer = 100;

    public float timer;

    void Start()
    {
        timer = throwCooldown;
        _animator = GetComponent<Animator>();
        currentHealth = startHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        //SetUpTree();
    }

    void Update()
    {
        SetUpTree();
        if (currentHealth < 1)
        {
            if (pool != null)
            {
                pool.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }

            return;
        }

        timer -= Time.deltaTime;
        // ToptreeNodeEvaluate();
    }

    protected override TreeNode SetUpTree()
    {
        ChaseTreeNodeRanged chaseTreeNodeMelee =
            new ChaseTreeNodeRanged(target, distanceToTargetPlayer, agent, _animator);

        RangeTreeNodeRange chasingRangeTreeNodeMelee =
            new RangeTreeNodeRange(target, distanceToTargetPlayer, chasingRange, _animator);

        RangeTreeNodeRange shootingRangeTreeNodeMelee =
            new RangeTreeNodeRange(target, distanceToTargetPlayer, rangedAttackRange, _animator);

        RangedAttackTreeNode rangedAttackTreeNode =
            new RangedAttackTreeNode(target, agent,
                throwabelObject, throwUpForce, throwForce, this);

        Sequence chaseSequence = new Sequence(new List<TreeNode> {chasingRangeTreeNodeMelee, chaseTreeNodeMelee});
        Sequence shootSequence = new Sequence(new List<TreeNode> {shootingRangeTreeNodeMelee, rangedAttackTreeNode});

        m_TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});

        return m_TopTreeNode;
    }


    public override void SetPool(IObjectPool<BaseClassEnemyAI> pool)
    {
        this.pool = pool;
    }

    public override void TargetPlayerPos(Vector3 TargetPos)
    {
        target = TargetPos;
    }

    public override void DistanceToPlayerPos(float distance)
    {
        distanceToTargetPlayer = distance;
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
    }
}