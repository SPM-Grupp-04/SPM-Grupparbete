using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace EnemyAI.RangedAI
{
    public class RangedAI : BaseEnemyAI, IDamagable
    {
        //  private TreeNode topTreeNode;
        //private float currentHealth;
        private NavMeshAgent agent;
        // [SerializeField] private float startHealth;
        [SerializeField] private float rangedAttackRange;
        //[SerializeField] private float movementSpeed;
        [SerializeField] private float chasingRange;
        //private Animator animator;
        private IObjectPool<BaseEnemyAI> pool;
    
        [Header("Throw Weapon settings")] [SerializeField]
        private GameObject throwabelObject;

        [SerializeField] private float throwCooldown;
        [SerializeField] private float throwUpForce;
        [SerializeField] private float throwForce = 30;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Animator animator;
        public Vector3 target = new Vector3(100, 0, 100);
        public float distanceToTargetPlayer = 100;
        public Vector3 playerPos = new Vector3(100, 0, 100);
        public float timer;

        void Start()
        {
            movementSpeed = 4;
            base.Start();
            timer = 0;
            
            currentHealth = startingHealth;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = movementSpeed;
            base.randomNumber = Random.Range(1, 10);
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
        }

        protected override TreeNode SetUpTree()
        {
            ChaseTreeNodeRanged chaseTreeNodeMelee =
                new ChaseTreeNodeRanged(target, distanceToTargetPlayer, agent, animator);

            RangeTreeNodeRange chasingRangeTreeNodeMelee =
                new RangeTreeNodeRange(target, distanceToTargetPlayer, chasingRange, animator);

            RangeTreeNodeRange shootingRangeTreeNodeMelee =
                new RangeTreeNodeRange(target, distanceToTargetPlayer, rangedAttackRange, animator);

            RangedAttackTreeNode rangedAttackTreeNode =
                new RangedAttackTreeNode(firePoint,playerPos, agent,
                    throwabelObject, throwUpForce, throwForce, this);
        
            Sequence chaseSequence = new Sequence(new List<TreeNode> {chasingRangeTreeNodeMelee, chaseTreeNodeMelee});
            Sequence shootSequence = new Sequence(new List<TreeNode> {shootingRangeTreeNodeMelee, rangedAttackTreeNode});

            TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});

            return TopTreeNode;
        }


        public override void SetPool(IObjectPool<BaseEnemyAI> pool)
        {
            this.pool = pool;
        }

        public override void PositionAroundTarget(Vector3 TargetPos)
        {
            target = TargetPos;
        }

        public override void DistanceToPlayerPos(float distance)
        {
            distanceToTargetPlayer = distance;
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
}