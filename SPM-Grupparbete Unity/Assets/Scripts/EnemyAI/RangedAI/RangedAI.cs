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
    private List<Transform> playerTransform = new List<Transform>();
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

    void Start()
    {
        playerTransform.Add(GameObject.Find("Player1").transform);
        playerTransform.Add(GameObject.Find("Player2").transform);
        
        _animator = GetComponent<Animator>();
        currentHealth = startHealth;
        agent = GetComponent<NavMeshAgent>();
        SetUpTree();
    }

    void Update()
    {
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

        // ToptreeNodeEvaluate();
      
    }

    protected override TreeNode SetUpTree()
    {
        ChaseTreeNodeRanged chaseTreeNodeMelee = new ChaseTreeNodeRanged(playerTransform, agent,_animator );

        RangeTreeNodeRange chasingRangeTreeNodeMelee = new RangeTreeNodeRange(chasingRange, playerTransform, transform, _animator);

        RangeTreeNodeRange shootingRangeTreeNodeMelee = new RangeTreeNodeRange(rangedAttackRange, playerTransform, transform, _animator);

        RangedAttackTreeNode meeleAttackTreeNode =
            new RangedAttackTreeNode(agent, gameObject, playerTransform,
                throwabelObject, throwCooldown, throwUpForce, throwForce);

        Sequence chaseSequence = new Sequence(new List<TreeNode> {chasingRangeTreeNodeMelee, chaseTreeNodeMelee});
        Sequence shootSequence = new Sequence(new List<TreeNode> {shootingRangeTreeNodeMelee, meeleAttackTreeNode});
        
        m_TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});
        
        return m_TopTreeNode;
    }


    public override void SetPool(IObjectPool<BaseClassEnemyAI> pool)
    {
        this.pool = pool;
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
    }
}