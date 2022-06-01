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
        private readonly Transform firePoint;

        private readonly GameObject throwableObject;
        private Vector3 currentVelocity;


        [Header("ThrowSettings")] 
        private const float ThrowCd = 7;
        private readonly float throwUpForce;
        private readonly float throwForce;
        private readonly RangedAI rangedAI;


        public RangedAttackTreeNode(Transform firePoint,Vector3 target, NavMeshAgent agent,
            GameObject throwableObject, float throwUpForce, float throwForce, RangedAI rangedAI)
        {
            this.firePoint = firePoint;
            this.agent = agent;
            this.target = target;
            this.throwableObject = throwableObject;
            
            this.rangedAI = rangedAI;
            this.throwUpForce = throwUpForce;
            this.throwForce = throwForce;
        }

        public override NodeState Evaluate()
        {
            agent.isStopped = true;
            Transform agentT = agent.transform;
            
            agentT.LookAt(target);

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