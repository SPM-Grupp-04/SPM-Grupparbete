using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.AI;


namespace Utility.EnemyAI
{
    public class BossRangeAttack : TreeNode
    {
        private GameObject rockThrow;
        NavMeshAgent  thisTransform;
        private Transform firePoint;
        private float throwUpForce;
        private float throwForce;

        private Animator animator;
        private float timer = 0;

        private float cooldown = 5;

        // Hämtas
        private Transform target = null;

        public BossRangeAttack(GameObject rockThrow, NavMeshAgent thisTransform, float throwUpForce, float throwForce,
            Transform firePoint, Animator  animator)
        {
            this.rockThrow = rockThrow;
            this.thisTransform = thisTransform;
            this.throwUpForce = throwUpForce;
            this.throwForce = throwForce;
            this.firePoint = firePoint;
            this.animator = animator;
        }

        public override NodeState Evaluate()
        {
            // Någonting att slänga mot.
            // Någonitng att slänga.
            target = (Transform) GetData("target");

            Debug.Log(target);
            timer -= Time.deltaTime;

            if (target == null || timer > 0)
            {
                return NodeState.FAILURE;
            }

            animator.SetBool("Run", false);
            thisTransform.transform.LookAt(target);

            if (timer <= 0)
            {
                ShootEventInfo rockThrow = new ShootEventInfo(firePoint, this.rockThrow, throwUpForce, throwForce);
                EventSystem.current.FireEvent(rockThrow);
                timer = cooldown;
            }

           

            return NodeState.RUNNING;
        }
    }
}