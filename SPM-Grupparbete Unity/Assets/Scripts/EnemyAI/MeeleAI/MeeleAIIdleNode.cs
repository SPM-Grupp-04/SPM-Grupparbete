
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.MeeleAI 
{
    public class MeeleAIIdleNode : TreeNode
    {
        private Animator animator;

       public MeeleAIIdleNode(Animator animator)
        {
            this.animator = animator;
        }
        public override NodeState Evaluate()
        {
            animator.SetTrigger("Idle");
            return NodeState.SUCCESS;
        }
    }
}