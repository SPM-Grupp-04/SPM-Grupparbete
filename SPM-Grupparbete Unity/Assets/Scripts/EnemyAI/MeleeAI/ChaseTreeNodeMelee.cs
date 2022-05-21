using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;


public class ChaseTreeNodeMelee : TreeNode
{
  // private Transform target;
   // private List<Transform> targets;
   //private float target;
    private NavMeshAgent agent;
    private Animator _animator;
    private Vector3 target;
    private float distanceToTarget;


    public ChaseTreeNodeMelee(Vector3 target,float distanceToTarget ,NavMeshAgent agent, Animator animator)
    {
        _animator = animator;
       // this.targets = targets;
        this.agent = agent;
        this.target = target;
        this.distanceToTarget = distanceToTarget;

    }

    public override NodeState Evaluate()
    {
        _animator.SetBool("Run", true);
        
        /*foreach (Transform target in targets)
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