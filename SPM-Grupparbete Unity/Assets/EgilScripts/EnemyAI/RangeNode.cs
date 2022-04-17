using BehaviorTree;
using UnityEngine;

public class RangeNode : EgilNode
{
    private float range;
    private Transform target;
    private Transform orgin;

    public RangeNode(float range, Transform target, Transform orgin)
    {
        this.range = range;
        this.target = target;
        this.orgin = orgin;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, orgin.position);

        state = distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}