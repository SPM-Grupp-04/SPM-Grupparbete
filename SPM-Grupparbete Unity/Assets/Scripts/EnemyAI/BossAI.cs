using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;
using UnityEngine;
using BehaviorTree;
using EnemyAI;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Utility.EnemyAI;
using Tree = BehaviorTree.Tree;

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

    [SerializeField] private float CheckForMeeleAttackFOV = 3f;
    [SerializeField] private float CheckForLaserFOV = 15;
    [SerializeField] private float CheckForRangeAttackFOV = 20;
    [SerializeField] private float CheckIfIShouldMoveToPlayerFOV = 30;
    [SerializeField] private float laerAttackRange;

    [SerializeField] private float throwForce = 30;
    [SerializeField] private float throwUpForce = 2;

    [SerializeField] private List<Transform> waypoints;

    [SerializeField] private MeleeWepon meleeWepon;

    private float currentHealth = 50;

    protected override TreeNode SetUpTree()
    {
        
        var agentTransform = agent.transform;
        TreeNode root = new Selector(new List<TreeNode>
        {
            // Kolla om spelaren är i range
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(this
                    , agentTransform, CheckForMeeleAttackFOV),
                new BossMeeleAttack(this, agent, animator, meleeWepon)
            }),

            // Skjuter spelaren.
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(this, agentTransform, CheckForLaserFOV),
                new BossAttackWithLaser(this, agent, lineRenderer, lineRendererTwo, firePoint,
                    firePointTwo, laerAttackRange, animator),
            }),

            // Slänger stenar mot spelaren.
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(this, transform, CheckForRangeAttackFOV),
                new BossRangeAttack(this, rockToThrow, agent, throwUpForce, throwForce, rockThrowPosition, animator)
            }),

            // Springer efters seplaren
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(this, agentTransform, CheckIfIShouldMoveToPlayerFOV),
                new BossMoveToPlayers(agent, animator),
            }),

            // Reset state.
            new BossIdle(agent, animator, waypoints)
        });

        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(agent.transform.position, CheckIfIShouldMoveToPlayerFOV);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(agent.transform.position, CheckForLaserFOV);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(agent.transform.position, CheckForRangeAttackFOV);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(agent.transform.position, CheckForMeeleAttackFOV);
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 1)
        {
            agent.speed = 0;
            StartCoroutine(waitbeforeDieWithoutPool());
        }
    }

    public float getCurrentHealth()
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