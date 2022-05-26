using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.MeeleAI
{
    public class ChaseTreeNodeMelee : TreeNode
    {
        private readonly NavMeshAgent agent;
        private readonly Animator animator;
        private readonly Vector3 target;
        private readonly float distanceToTarget;
    
        public ChaseTreeNodeMelee(Vector3 target,float distanceToTarget ,NavMeshAgent agent, Animator animator)
        {
            this.animator = animator;
            this.agent = agent;
            this.target = target;
            this.distanceToTarget = distanceToTarget;
        }

        public override NodeState Evaluate()
        {
            animator.SetBool("Run", true);
        
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
}