using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.RangedAI
{
    public class ChaseTreeNodeRanged : TreeNode
    {
        private Vector3 target;
        private float distanceToTarget;
        private NavMeshAgent agent;
        private Animator animator;


        public ChaseTreeNodeRanged(Vector3 target,float distanceToTarget, NavMeshAgent agent, Animator animator)
        {
            this.animator = animator;
            this.target = target;
            this.distanceToTarget = distanceToTarget;
            this.agent = agent;
        
        }

        public override NodeState Evaluate()
        {
            if (distanceToTarget > 15f)
            {
                agent.isStopped = false;
                agent.SetDestination(target);
                animator.SetBool("Run",true);

                return NodeState.RUNNING;
            }
            else
            {
                animator.SetBool("Run",false);
                agent.isStopped = true;
                state = NodeState.SUCCESS;
                return state;
            }
        }
    
    
    }
}