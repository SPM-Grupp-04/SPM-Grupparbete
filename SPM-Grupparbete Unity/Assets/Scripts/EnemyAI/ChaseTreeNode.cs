using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;


public class ChaseTreeNode : TreeNode
{
    private Transform target;
    private Transform[] targets;
    private NavMeshAgent agent;


    public ChaseTreeNode(Transform[] targets, NavMeshAgent agent)
    {
        this.targets = targets;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        float distance = 100;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, agent.transform.position);

            if (tempdistance < distance && target.gameObject.activeInHierarchy)
            {
                distance = tempdistance;
                this.target = target;
            }
        }

        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);

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