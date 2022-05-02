using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;


public class MeeleEnemyAI : BaseClassEnemyAI, IDamagable
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
    [SerializeField] private float movementSpeed;

    [SerializeField] private MeleeWepon _meleeWepon;
    
    private List<Transform> playerTransform = new List<Transform>();

    private IObjectPool<BaseClassEnemyAI> pool;

    private Animator _animator;

    private NavMeshAgent agent;
    private TreeNode m_TopTreeNode;


    void Start()
    {
        _animator = GetComponent<Animator>();
        playerTransform.Add(GameObject.Find("Player1").transform);
        playerTransform.Add(GameObject.Find("Player2").transform);

        agent.speed = movementSpeed;
        base.Start();
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
            if (this.pool != null)
            {
                //   _meshRenderer.enabled = false;
                pool.Release(this);
            }
            else
            {
                Debug.Log("Pool is null");
                gameObject.SetActive(false);
            }

            return;
        }

        m_TopTreeNode.Evaluate();
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    protected override TreeNode SetUpTree()
    {
        //IsCoverAvaliableTreeNode coverAvaliableNode = new IsCoverAvaliableTreeNode(avaliableCovers, playerTransform, this);
        // GoToCoverTreeNode goToCoverNode = new GoToCoverTreeNode(agent, this);
        // HealthTreeNode healthTreeNode = new HealthTreeNode(this, lowHealthThreseHold);
        //  IsCoverdTreeNode isCoveredTreeNode = new IsCoverdTreeNode(playerTransform, transform);

        ChaseTreeNode chaseTreeNode = new ChaseTreeNode(playerTransform, agent, _animator); // Animator.

        RangeTreeNode chasingRangeTreeNode = new RangeTreeNode(chasingRange, playerTransform, transform);

        RangeTreeNode shootingRangeTreeNode = new RangeTreeNode(shootingRange, playerTransform, transform);

        MeeleAttackTreeNode meeleAttackTreeNode = new MeeleAttackTreeNode(agent, gameObject, playerTransform, _animator,_meleeWepon);


        Sequence chaseSequence = new Sequence(new List<TreeNode> {chasingRangeTreeNode, chaseTreeNode});
        Sequence shootSequence = new Sequence(new List<TreeNode> {shootingRangeTreeNode, meeleAttackTreeNode});

        //Sequence goToCoverSequence = new Sequence(new List<TreeNode> {coverAvaliableNode, goToCoverNode});
        //Selector findCoverSelector = new Selector(new List<TreeNode> {goToCoverSequence, chaseSequence});
        //  Selector tryToTakeCoverSelector = new Selector(new List<TreeNode> {isCoveredTreeNode, findCoverSelector});
        //Sequence mainCoverSequence = new Sequence(new List<TreeNode> {healthTreeNode, tryToTakeCoverSelector});

        //m_TopTreeNode = new Selector(new List<TreeNode> {mainCoverSequence, shootSequence, chaseSequence});
        m_TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});

        return m_TopTreeNode;
    }


    public override void SetPool(IObjectPool<BaseClassEnemyAI> pool)
    {
        // Inte null

        this.pool = pool;

//        Debug.Log("MelleEnemyAI POOL in SetPool " + this.pool);
    }

    public void DealDamage(int damage)
    {
        CurrentHealth -= damage;
    }
}