using System.Collections.Generic;
using BehaviorTree;
using EnemyAI.MeeleAI;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public class BossMeleeAttack : TreeNode
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
        private readonly Animator animator;
        private readonly MeleeWepon meleeWeapon;
        private readonly BossAI bossAI;
        
        public BossMeleeAttack(BossAI bossAI,NavMeshAgent agent, Animator animator, MeleeWepon meleeWeapon)
        {
            this.agent = agent;
            this.animator = animator;
            this.meleeWeapon = meleeWeapon;
            this.bossAI = bossAI;
        }

        public override NodeState Evaluate()
        {
            if (bossAI.GetCurrentHealth() < 1)
            {
                Debug.Log("Boss return Fail In Melee");
                return NodeState.FAILURE;
            }
            Debug.Log("SHould stand still and hit.");
            agent.isStopped = true;
            animator.SetBool("Run", false);
            var agentT = agent.transform;
            var target = (Transform) GetData("target");
            agentT.LookAt(target);
           // if (meleeWeapon.timeRemaining <= 0.1f)
           // {
                animator.SetTrigger("Attack");
           // }

            return NodeState.RUNNING;
        }
    }
}