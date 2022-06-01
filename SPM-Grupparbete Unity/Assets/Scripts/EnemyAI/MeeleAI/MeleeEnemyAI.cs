using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace EnemyAI.MeeleAI
{
    public class MeleeEnemyAI : BaseEnemyAI, IDamagable
    {

        [SerializeField] private float chasingRange;
        [SerializeField] private float hitRange;
        [SerializeField] private MeleeWepon meleeWeapon;
        [SerializeField] private Animator animator;
      
        private IObjectPool<BaseEnemyAI> pool;
       
       
        private NavMeshAgent agent;
      
        
        public Vector3 target = new Vector3(100, 0, 100);
        public float distanceToTarget = 100;
        public Vector3 playerPos = new Vector3(100, 0, 100);
        private float timeUntilAnimationPlay;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
         
        }

        void Start()
        {
            animator.speed = Random.Range(0.9f, 1.3f);
            base.Start();
            base.randomNumber = Random.Range(1, 10);
            agent.radius = Random.Range(0.3f, 0.7f);
            movementSpeed = Random.Range(6,7);
            agent.autoBraking = true;
            agent.speed = movementSpeed;
            currentHealth = startingHealth;
        }


        private void Update()
        {
            SetUpTree();

            if (currentHealth < 1)
            {
                agent.speed = 0;
                animator.SetTrigger("Die");
            }
          
            
        }
        
        public void DieAnimationEvent()
        {
            if (this.pool != null)
            {
                pool.Release(this);
               
            }
            else
            {
                gameObject.SetActive(false);
                
            }
            agent.speed = movementSpeed;
            currentHealth = startingHealth;
        }
        

        

        protected override TreeNode SetUpTree()
        {
            // Setting up nodes
            ChaseTreeNodeMelee chaseTreeNodeMelee =
                new ChaseTreeNodeMelee(target, distanceToTarget, agent, animator); 

            RangeTreeNodeMelee inChaseRange =
                new RangeTreeNodeMelee(chasingRange, distanceToTarget, animator);

            RangeTreeNodeMelee inMeleeRange =
                new RangeTreeNodeMelee(hitRange, distanceToTarget, animator);

            MeeleAttackTreeNode meeleAttackTreeNode =
                new MeeleAttackTreeNode(playerPos, agent, animator, meleeWeapon);
            // 
        
            // Setting up sequence.
            Sequence chaseSequence = new Sequence(new List<TreeNode> {inChaseRange, chaseTreeNodeMelee});
     
            Sequence shootSequence = new Sequence(new List<TreeNode> {inMeleeRange, meeleAttackTreeNode});

            // Choosing one of de sequence taking the first one that has returned Success/Running. Order is important 
            TopTreeNode = new Selector(new List<TreeNode> {shootSequence, chaseSequence});
        
            return TopTreeNode;
        }

        public override void SetPool(IObjectPool<BaseEnemyAI> pool)
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

        public void tryToHitPlayer()
        {
            meleeWeapon.HitPlayer();
        }
        public void DealDamage(float damage)
        {
            currentHealth -= damage;
        }
    }
}