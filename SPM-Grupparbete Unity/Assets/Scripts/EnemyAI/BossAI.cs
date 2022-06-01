using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using EnemyAI.MeeleAI;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

namespace EnemyAI
{
    public class BossAI : Tree, IDamagable
    {
        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private GameObject rockToThrow;

        [SerializeField] private Animator animator;

        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LineRenderer lineRendererTwo;

        [SerializeField] private Transform rockThrowPosition;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform firePointTwo;

        [SerializeField] private float checkForMeleeAttackFOV = 3f;
        [SerializeField] private float checkForLaserFOV = 15;
        [SerializeField] private float checkForRangeAttackFOV = 20;
        [SerializeField] private float checkIfIShouldMoveToPlayerFOV = 30;
        [SerializeField] private float laserAttackRange;

        [SerializeField] private float throwForce = 30;
        [SerializeField] private float throwUpForce = 2;

        [SerializeField] private List<Transform> waypoints;

        [SerializeField] private MeleeWepon meleeWeapon;

        [SerializeField] private float currentHealth = 50;


        public void tryToHitPlayer()
        {
            meleeWeapon.HitPlayer();
        }

        protected override TreeNode SetUpTree()
        {
            var agentTransform = agent.transform;
            TreeNode root = new Selector(new List<TreeNode>
            {
                // Kolla om spelaren är i range
                new Sequence(new List<TreeNode>
                {
                    new CheckPlayerInAttackRange(this
                        , agentTransform, checkForMeleeAttackFOV),
                    new BossMeleeAttack(this, agent, animator, meleeWeapon)
                }),

                // Skjuter spelaren.
                new Sequence(new List<TreeNode>
                {
                    new CheckPlayerInAttackRange(this, agentTransform, checkForLaserFOV),
                    new BossLaserAttack(this, agent, lineRenderer, lineRendererTwo, firePoint,
                        firePointTwo, laserAttackRange, animator),
                }),

                // Slänger stenar mot spelaren.
                new Sequence(new List<TreeNode>
                {
                    new CheckPlayerInAttackRange(this, transform, checkForRangeAttackFOV),
                    new BossRangeAttack(this, rockToThrow, agent, throwUpForce, throwForce, rockThrowPosition, animator)
                }),

                // Springer efters seplaren
                new Sequence(new List<TreeNode>
                {
                    new CheckPlayerInAttackRange(this, agentTransform, checkIfIShouldMoveToPlayerFOV),
                    new BossMoveToPlayers(agent, animator),
                }),

                // Reset state.
                new BossIdle(agent, animator, waypoints)
            });

            return root;
        }

        public void DealDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth < 1)
            {
                agent.speed = 0;
                animator.SetTrigger("Die");
                //StartCoroutine(waitbeforeDieWithoutPool());
            }
        }

        public void DieAnimationEvent()
        {
            gameObject.SetActive(false);
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        private IEnumerator waitbeforeDieWithoutPool()
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(2);

            gameObject.SetActive(false);
        }
    }
}