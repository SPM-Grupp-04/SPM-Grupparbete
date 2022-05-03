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

    [SerializeField] private float healthRestoreRate;
    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;
    [SerializeField] private float movementSpeed;

    [SerializeField] private MeleeWepon _meleeWepon;

    private List<Transform> playerTransform = new List<Transform>();

    private IObjectPool<BaseClassEnemyAI> pool;

    private Animator _animator;

    private NavMeshAgent agent;
    //public TreeNode m_TopTreeNode;


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
                currentHealth = startingHealth;
                pool.Release(this);
            }
            else
            {
                Debug.Log("Pool is null");
                gameObject.SetActive(false);
            }

            return;
        }

        // m_TopTreeNode.Evaluate();
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    protected override TreeNode SetUpTree()
    {
        ChaseTreeNodeMelee chaseTreeNodeMelee = new ChaseTreeNodeMelee(playerTransform, agent, _animator); // Animator.

        RangeTreeNodeMelee inChaseRange = new RangeTreeNodeMelee(chasingRange, playerTransform, transform, _animator);

        RangeTreeNodeMelee inMeleeRange = new RangeTreeNodeMelee(shootingRange, playerTransform, transform, _animator);

        MeeleAttackTreeNode meeleAttackTreeNode =
            new MeeleAttackTreeNode(agent, gameObject, playerTransform, _animator, _meleeWepon);

        Sequence chaseSequence = new Sequence(new List<TreeNode> {inChaseRange, chaseTreeNodeMelee});
        Sequence shootSequence = new Sequence(new List<TreeNode> {inMeleeRange, meeleAttackTreeNode});

        m_TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});


        return m_TopTreeNode;
    }


    public override void SetPool(IObjectPool<BaseClassEnemyAI> pool)
    {
        this.pool = pool;
    }

    public void DealDamage(int damage)
    {
        CurrentHealth -= damage;
    }
}