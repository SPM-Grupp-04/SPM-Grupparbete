using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class RangeTreeNodeRange : TreeNode
{
    private float range;
    private Vector3 target;
    private float distanceToTarget;


    private Animator _animator;

    public RangeTreeNodeRange(Vector3 target, float distanceToTarget, float range, Animator animator)
    {
        this.range = range;
        this.target = target;
        this.distanceToTarget = distanceToTarget;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        /*float distance = largeDistanceNumber;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, orgin.position);

            if (tempdistance < distance && target.gameObject.activeInHierarchy)
            {
                distance = tempdistance;
                this.target = target;
            }

                
        }*/

        state = distanceToTarget <= range ? NodeState.SUCCESS : NodeState.FAILURE;

        return state;
    }
}