using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;


public class ChaseTreeNodeRanged : TreeNode
{
    private Vector3 target;
    private float distanceToTarget;
    private NavMeshAgent agent;
    private Animator _animator;


    public ChaseTreeNodeRanged(Vector3 target,float distanceToTarget, NavMeshAgent agent, Animator animator)
    {
        _animator = animator;
        this.target = target;
        this.distanceToTarget = distanceToTarget;
        this.agent = agent;
        
    }

    public override NodeState Evaluate()
    {
        
        /*float distance = 100;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, agent.transform.position);

            if (tempdistance < distance && target.gameObject.activeInHierarchy)
            {
                distance = tempdistance;
                this.target = target;
            }
        }*/

        if (distanceToTarget > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(target);

            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            state = NodeState.SUCCESS;
            return state;
        }
    }
    
    
}