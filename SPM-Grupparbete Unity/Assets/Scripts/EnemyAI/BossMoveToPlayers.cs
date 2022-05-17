using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class BossMoveToPlayers : TreeNode
{
    
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;
    private int moveSpeed = 5;

    public BossMoveToPlayers(NavMeshAgent agent, Animator animator)
    {
        this.agent = agent;
        this.animator = animator;
        
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        
        if (t != null)
        {
            agent.speed = moveSpeed;
            agent.isStopped = false;
            
            animator.SetBool("Run", true);
            target = (Transform) GetData("target");
            
            agent.SetDestination(new Vector3(target.position.x, agent.transform.position.y, target.position.z));
            Debug.Log( agent.SetDestination(new Vector3(target.position.x, agent.transform.position.y, target.position.z)) + " Agents Destination");
          //  agent.transform.LookAt(target);
           
            return NodeState.SUCCESS;
        }
        
        animator.SetBool("Run", false);
        ClearData("target");

        return NodeState.FAILURE;
    }
}