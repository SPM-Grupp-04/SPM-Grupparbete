using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI.RangedAI
{
    public class RangedAttackTreeNode : TreeNode
    {
        private readonly NavMeshAgent agent;
        private readonly Vector3 target;
       

        
        private Vector3 currentVelocity;


        [Header("ThrowSettings")] 
        private const float ThrowCd = 7;
        private readonly float throwUpForce;
        private readonly float throwForce;
        private readonly RangedAI rangedAI;
        private Animator animator;


        public RangedAttackTreeNode( NavMeshAgent agent
           , RangedAI rangedAI, Vector3 target, Animator animator)
        {
            this.target = target;
            this.agent = agent;
            this.rangedAI = rangedAI;
            this.animator = animator;
        }

        public override NodeState Evaluate()
        {
            agent.isStopped = true;
            Transform agentT = agent.transform;
            
            agentT.LookAt(target);
            
            animator.SetTrigger("Throw");
            
            // Set Animation. 
            
            /*if (rangedAI.timer < 0)
            {
                /*var shootEventInfo = new ShootEventInfo(firePoint,
                    this.throwableObject, throwUpForce, throwForce);
                EventSystem.current.FireEvent(shootEventInfo);#1#
                rangedAI.timer = ThrowCd;
            }*/
            
            state = NodeState.RUNNING;
            return state;
        }
        
        
    }
}