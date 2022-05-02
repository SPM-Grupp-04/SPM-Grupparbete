using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class RangeTreeNodeMelee : TreeNode
{
    
    /*
     * Klassen kollar om ett target finns inom det in skickade värdet. 
     */
    private float range;
    private Transform target;
    private List<Transform> targets;
    private Transform orgin;
    private const int largeDistanceNumber = 100;
    private Animator _animator;
    public RangeTreeNodeMelee(float range, List<Transform> targets, Transform orgin, Animator animator)
    {
        this.range = range;
        this.targets = targets;
        this.orgin = orgin;
        _animator = animator;
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
        if (state == NodeState.FAILURE)
        {
            _animator.SetBool("Run",false);
        }
        return state;
    }
}