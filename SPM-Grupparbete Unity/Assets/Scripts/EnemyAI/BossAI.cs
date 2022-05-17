using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEditor;
using UnityEngine;
using BehaviorTree;
using EnemyAI;
using UnityEngine.AI;
using Utility.EnemyAI;
using Tree = BehaviorTree.Tree;

public class BossAI : Tree
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject rockToThrow;

    [SerializeField] private Animator animator;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer lineRendererTwo;

    [SerializeField] private Transform rockThrowPosition;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePointTwo;

    private const float CheckForMeeleAttackFOV = 3f;
    private const float CheckForLaserFOV = 15;
    private const float CheckForRangeAttackFOV = 20;
    private const float CheckIfIShouldMoveToPlayerFOV = 30;

    [SerializeField] private float fovAttackRange;
    [SerializeField] private float throwForce = 30;
    [SerializeField] private float throwUpForce = 2;

    [SerializeField] private List<Transform> waypoints;

    [SerializeField] private MeleeWepon meleeWepon;

    protected override TreeNode SetUpTree()
    {
        var agentTransform = agent.transform;
        TreeNode root = new Selector(new List<TreeNode>
        {
            // Kolla om spelaren är i range
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(agentTransform, CheckForMeeleAttackFOV),
                new BossMeeleAttack(agent, animator, meleeWepon)
            }),
          
            // Skjuter spelaren.
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(agentTransform, CheckForLaserFOV),
                new BossAttackWithLaser(agent, lineRenderer, lineRendererTwo, firePoint, 
                    firePointTwo,fovAttackRange,animator),
            }),
            
            // Slänger stenar mot spelaren.
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(transform, CheckForRangeAttackFOV),
                new BossRangeAttack(rockToThrow, agent, throwUpForce, throwForce, rockThrowPosition, animator)
            }),
            
            // Springer efters seplaren
            new Sequence(new List<TreeNode>
            {
                new CheckPlayerInAttackRange(agentTransform, CheckIfIShouldMoveToPlayerFOV),
                new BossMoveToPlayers(agent, animator),
            }),
           
            // Reset state.
            new BossIdle(agent, animator, waypoints)
            
        });
        
        return root;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(agent.transform.position,CheckIfIShouldMoveToPlayerFOV);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(agent.transform.position, CheckForLaserFOV);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(agent.transform.position, CheckForRangeAttackFOV);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(agent.transform.position, CheckForMeeleAttackFOV);
    }
    
}