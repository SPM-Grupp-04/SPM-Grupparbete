using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;


public class MeleeEnemyAI : BaseClassEnemyAI, IDamagable
{
    [SerializeField] private float startingHealth;
    private float currentHealth;

    [SerializeField] private float healthRestoreRate;
    [SerializeField] private float chasingRange;
    [SerializeField] private float hitRange;
    [SerializeField] private float movementSpeed;

    [SerializeField] private MeleeWepon meleeWeapon;

    private List<Transform> playerTransform = new List<Transform>();

    private IObjectPool<BaseClassEnemyAI> pool;

    private Animator animator;

    private NavMeshAgent agent;
    //public TreeNode m_TopTreeNode;

    public Vector3 target = new Vector3(100, 0, 100);
    public float distanceToTargetPlayer = 100;
    public Vector3 playerPos = new Vector3(100, 0, 100);
    private float timeUntilAnimationPlay;

    new void Start()
    {
        base.Start();
        base.randomNumber = Random.Range(1, 10);
        animator = GetComponent<Animator>();
        // playerTransform.Add(GameObject.Find("Player1").transform);
        // playerTransform.Add(GameObject.Find("Player2").transform);

        agent.speed = movementSpeed;
        base.Start();
        currentHealth = startingHealth;
    }


    private void Update()
    {
        SetUpTree();
       
        if (currentHealth < startingHealth)
        {
            currentHealth += Time.deltaTime * healthRestoreRate;
        }

        if (currentHealth <= 1)
        {
            if (this.pool != null)
            {
                currentHealth = startingHealth;
              //  _animator.SetTrigger("Die");
              agent.speed = 0;
              //StartCoroutine(WaitBeforeDie());
              //  pool.Release(this);
            }
            else
            {
                Debug.Log("Pool is null");
                agent.speed = 0;
                //StartCoroutine(WaitBeforeDieWithoutPool());
                // StartCoroutine(WaitBeforeDie());
                // gameObject.SetActive(false);
            }
        }

        // m_TopTreeNode.Evaluate();
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    protected override TreeNode SetUpTree()
    {
        // Istället för att skcika med spelarens transform hela tiden så skicka bara med platsen.

        
        ChaseTreeNodeMelee chaseTreeNodeMelee =
            new ChaseTreeNodeMelee(target, distanceToTargetPlayer, agent, animator); // Animator.

        RangeTreeNodeMelee inChaseRange =
            new RangeTreeNodeMelee(chasingRange,  distanceToTargetPlayer, animator);

        RangeTreeNodeMelee inMeleeRange =
            new RangeTreeNodeMelee(hitRange,  distanceToTargetPlayer, animator);

        MeleeAttackTreeNode meeleAttackTreeNode =
            new MeleeAttackTreeNode(target, agent, animator, meleeWeapon);

        Sequence chaseSequence = new Sequence(new List<TreeNode> {inChaseRange, chaseTreeNodeMelee});
        Sequence shootSequence = new Sequence(new List<TreeNode> {inMeleeRange, meeleAttackTreeNode});

        MTopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});

        return MTopTreeNode;
    }


    public override void SetPool(IObjectPool<BaseClassEnemyAI> pool)
    {
        this.pool = pool;
    }

    public void TargetPlayerPos(Vector3 targeDistance)
    {
        target = targeDistance;
    }

    public override void DistanceToPlayerPos(float distance)
    {
        this.distanceToTargetPlayer = distance;
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
    }

    public override void PositionAroundTarget(Vector3 TargetPos)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerPos(Vector3 playerPos)
    {
        throw new System.NotImplementedException();
    }

    public override float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DealDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}