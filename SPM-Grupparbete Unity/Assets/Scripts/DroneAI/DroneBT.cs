using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Utility.DroneAI;
using Tree = BehaviorTree.Tree;

public class DroneBT : Tree
{
    //Grund klassen för en drönare. Hör byggs trädet upp och variablar som alla drönare behöver läggstill.
    public const float fov = 8f;
    public const float fovAttackRange = 5f;
    public const float fovMiningRange = 4f;
    public const float droneSpeed = 4;

    [SerializeField] private Transform partToRotate;
    [SerializeField] private LayerMask ignorLayers;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform firepoint;

    protected override TreeNode SetUpTree()
    {
        TreeNode root = new Selector(
            new List<TreeNode> // Sätter roten till den sequens som är korrekt eller till att man följer spelaren.
                {
                    new Sequence(new List<TreeNode>
                        {
                            new CheckEnemyInAttackRange(transform),
                            new AttackTarget(transform, partToRotate, lineRenderer, firepoint, fovAttackRange),
                        }
                    ),

                    new Sequence(new List<TreeNode>
                        {
                            new CheckIfEnemyIsInRange(transform),
                            new GoToTarget(transform),
                        }
                    ),

                    new Sequence(new List<TreeNode>
                        {
                            new CheckIfKristalIsInRange(transform, fovMiningRange),
                            new AttackTarget(transform, partToRotate, lineRenderer, firepoint, fovMiningRange)
                        }
                    ),

                    new Sequence(new List<TreeNode>
                        {
                            new CheckIfKristalIsInRange(transform, fov),
                            new GoToTarget(transform),
                        }
                    ),

                    // New Sequence kolla om det finns kristaller i närheten.
                    // Om det finns gå till dessa och collecta dom.

                    new FollowPlayer(transform, playerTransform), // Fall back Action.
                }
            );

        return root;
    }
}