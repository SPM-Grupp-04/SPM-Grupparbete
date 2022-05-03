﻿using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class RangeTreeNodeMelee : TreeNode
{
    /*
     * Klassen kollar om ett target finns inom det in skickade värdet. 
     */
    private float range;
    // private Transform target;
    // private List<Transform> targets;
    //private Transform orgin;

    private Animator _animator;

    private float distanceToPlayer;

    public RangeTreeNodeMelee(float range, float distanceToPlayer, Animator animator)
    {
        this.range = range;
        _animator = animator;

        this.distanceToPlayer = distanceToPlayer;
    }

    public override NodeState Evaluate()
    {
        /*float distance = largeDistanceNumber;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, orgin.position);
            Debug.Log(target + "RangeNode");
            if (tempdistance < distance && target.gameObject.activeInHierarchy)
            {
                distance = tempdistance;
               
                this.target = target;
            }

                
        }*/

        state = distanceToPlayer <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        if (state == NodeState.FAILURE)
        {
            _animator.SetBool("Run", false);
        }

        return state;
    }
}