using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public class BossMeeleAttack : TreeNode
    {
        /*
     * Klassen gör så att fienden vänder sig mot sitt mål och sedan attackerar det. Här väljer den också
     * Vilket mål den ska attackera
     * 
     */
        private NavMeshAgent agent;

        private GameObject gameObject;
        
        private List<Transform> targets;
        private Vector3 currentVelocity;
        private Animator animator;
        private MeleeWepon meleeWepon;
        private BossAI bossAI;
        
        public BossMeeleAttack(BossAI bossAI,NavMeshAgent agent, Animator animator, MeleeWepon meleeWepon)
        {
            this.agent = agent;
            this.animator = animator;
            this.meleeWepon = meleeWepon;
            this.bossAI = bossAI;
        }

        public override NodeState Evaluate()
        {
            if (bossAI.getCurrentHealth() < 1)
            {
                return NodeState.FAILURE;
            }
            
            agent.isStopped = true;
            var agentT = agent.transform;
            var target = (Transform) GetData("target");
            agentT.LookAt(target);
            if (meleeWepon.timeRemaining <= 0.1f)
            {
                animator.SetTrigger("Attack");
            }

            return NodeState.RUNNING;
        }
    }
}