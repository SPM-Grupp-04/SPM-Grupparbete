using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public class BossRangeAttack : TreeNode
    {
        private GameObject rockThrow;
        NavMeshAgent thisTransform;
        private Transform firePoint;
        private float throwUpForce;
        private float throwForce;

        private Animator animator;
        private float timer = 0;

        private float cooldown = 5;

        // Hämtas
        private Transform target = null;
        private BossAI bossAI;

        public BossRangeAttack(BossAI bossAI,GameObject rockThrow, NavMeshAgent thisTransform, float throwUpForce, float throwForce,
            Transform firePoint, Animator animator)
        {
            this.rockThrow = rockThrow;
            this.thisTransform = thisTransform;
            this.throwUpForce = throwUpForce;
            this.throwForce = throwForce;
            this.firePoint = firePoint;
            this.animator = animator;
            this.bossAI = bossAI;
        }

        public override NodeState Evaluate()
        {
            target = (Transform) GetData("target");

            timer -= Time.deltaTime;

            if (target == null || timer > 0)
            {
                thisTransform.isStopped = false;
                return NodeState.FAILURE;
            }

            animator.SetBool("Run", false);
            thisTransform.transform.LookAt(target);

            if (timer <= 0)
            {
                var rockThrowEvent = new ShootEventInfo(firePoint, this.rockThrow, throwUpForce, throwForce);
                EventSystem.current.FireEvent(rockThrowEvent);
                timer = cooldown;
                return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }
    }
}