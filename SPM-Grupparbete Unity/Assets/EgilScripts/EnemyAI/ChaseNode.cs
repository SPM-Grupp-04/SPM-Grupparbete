using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;


public class ChaseNode : EgilNode
{
    private Transform target;
    private Transform[] targets;
    private NavMeshAgent agent;
    private EnemyAI ai;

    public ChaseNode(Transform[] targets, NavMeshAgent agent, EnemyAI ai)
    {
        this.targets = targets;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);

        float distance = 100;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, agent.transform.position);

            if (tempdistance < distance)
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