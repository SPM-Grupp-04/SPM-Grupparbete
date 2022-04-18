using System.Diagnostics;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;


public class ShootNode : EgilNode
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private Transform target;
    private Transform[] targets;
    private Vector3 orgin;
    
    private Vector3 currentVelocity;
    private float smoothDamp;

    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform[] targets, Vector3 orgin)
    {
        this.agent = agent;
        this.orgin = orgin;
        this.ai = ai;
        this.targets = targets;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {

        foreach (Transform target in targets)
        {
            this.target = target;
        }
        float distance = 100;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, orgin);

            if (tempdistance < distance)
            {
                distance = tempdistance;
                this.target = target;
            }
               
        }
        
        
        agent.isStopped = true;
        ai.SetColor(Color.green);
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;

        state = NodeState.RUNNING;
        return state;
    }
}