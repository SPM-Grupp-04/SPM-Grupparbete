using BehaviorTree;
using UnityEngine;

public class RangeNode : EgilNode
{
    private float range;
    private Transform target;
    private Transform[] targets;
    private Transform orgin;

    public RangeNode(float range, Transform[] targets, Transform orgin)
    {
        this.range = range;
        this.targets = targets;
        this.orgin = orgin;
    }

    public override NodeState Evaluate()
    {
        float distance = 100;
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