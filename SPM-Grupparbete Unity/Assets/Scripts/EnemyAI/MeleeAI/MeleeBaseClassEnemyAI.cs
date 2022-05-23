using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;


public class MeleeBaseClassEnemyAI : BaseClassEnemyAI, IDamagable
{
    [SerializeField] private float startingHealth;
    private float currentHealth;

    [SerializeField] private float healthRestoreRate;
    [SerializeField] private float chasingRange;
    [SerializeField] private float hitRange;
    [SerializeField] private float movementSpeed;

    [SerializeField] private MeleeWepon _meleeWepon;

    private List<Transform> playerTransform = new List<Transform>();

    private IObjectPool<BaseClassEnemyAI> pool;

    private Animator _animator;

    private NavMeshAgent agent;
    //public TreeNode m_TopTreeNode;

    public Vector3 target = new Vector3(100, 0, 100);
    public float distanceToTarget = 100;
    public Vector3 playerPos = new Vector3(100, 0, 100);
    private float timeUntillAnimationPlay;

    void Start()
    {
        base.Start();
        base.randomNumber = Random.Range(1, 10);

        _animator = GetComponent<Animator>();
        // playerTransform.Add(GameObject.Find("Player1").transform);
        // playerTransform.Add(GameObject.Find("Player2").transform);

        agent.speed = movementSpeed;
        base.Start();
        currentHealth = startingHealth;


        // SetUpTree();
        // SetUpTree();
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
           
           // StartCoroutine(WaitBeforeDie());

            if (this.pool != null)
            {
                currentHealth = startingHealth;
              //  _animator.SetTrigger("Die");
              agent.speed = 0;
              StartCoroutine(WaitBeforeDie());
              //  pool.Release(this);
            }
            else
            {
                Debug.Log("Pool is null");
                agent.speed = 0;
                StartCoroutine(waitbeforeDieWithoutPool());
                // StartCoroutine(WaitBeforeDie());
                // gameObject.SetActive(false);
            }
        }

        // m_TopTreeNode.Evaluate();
    }

    private IEnumerator waitbeforeDieWithoutPool()
    {
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(2);
        agent.speed = movementSpeed;
        gameObject.SetActive(false);
    }
    private IEnumerator WaitBeforeDie()
    {
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(2);
        agent.speed = movementSpeed;
        pool.Release(this);
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    protected override TreeNode SetUpTree()
    {
        // Istället för att skcika med spelarens transform hela tiden så skicka bara med platsen.

        ChaseTreeNodeMelee chaseTreeNodeMelee =
            new ChaseTreeNodeMelee(target, distanceToTarget, agent, _animator); // Animator.

        RangeTreeNodeMelee inChaseRange =
            new RangeTreeNodeMelee(chasingRange, distanceToTarget, _animator);

        RangeTreeNodeMelee inMeleeRange =
            new RangeTreeNodeMelee(hitRange, distanceToTarget, _animator);

        MeleeAttackTreeNode meeleAttackTreeNode =
            new MeleeAttackTreeNode(playerPos, agent, _animator, _meleeWepon);


        Sequence chaseSequence = new Sequence(new List<TreeNode> {inChaseRange, chaseTreeNodeMelee});
        Sequence shootSequence = new Sequence(new List<TreeNode> {inMeleeRange, meeleAttackTreeNode});

        MTopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});


        return MTopTreeNode;
    }

    public override void SetPool(IObjectPool<BaseClassEnemyAI> pool)
    {
        this.pool = pool;
    }

    public override void PositionAroundTarget(Vector3 targeDistance)
    {
        target = targeDistance;
    }

    public override void DistanceToPlayerPos(float distance)
    {
        this.distanceToTarget = distance;
    }

    public override void PlayerPos(Vector3 playerPos)
    {
        this.playerPos = playerPos;
    }

    public override float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
    }
}