using BehaviorTree;
using UnityEngine;

public class RangeTreeNode : TreeNode
{
    private float range;
    private Transform target;
    private Transform[] targets;
    private Transform orgin;
    private const int largeDistanceNumber = 100;

    public RangeTreeNode(float range, Transform[] targets, Transform orgin)
    {
        this.range = range;
        this.targets = targets;
        this.orgin = orgin;
    }

    public override NodeState Evaluate()
    {
        float distance = largeDistanceNumber;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, orgin.position);

            if (tempdistance < distance && target.gameObject.activeInHierarchy)
            {
                distance = tempdistance;
                this.target = target;
            }

                
        }
        
        state = distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}