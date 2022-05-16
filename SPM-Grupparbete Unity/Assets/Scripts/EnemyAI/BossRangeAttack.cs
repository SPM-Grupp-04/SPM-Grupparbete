using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEditor.TextCore.Text;
using UnityEngine;


namespace Utility.EnemyAI
{
    public class BossRangeAttack : TreeNode
    {
        private GameObject rockThrow;
        private Transform thisTransform;
        private Transform firePoint;
        private float throwUpForce;
        private float throwForce;

        private float timer = 0;

        private float cooldown = 3;

        // Hämtas
        private Transform target = null;

        public BossRangeAttack(GameObject rockThrow, Transform thisTransform, float throwUpForce, float throwForce,
            Transform firePoint)
        {
            this.rockThrow = rockThrow;
            this.thisTransform = thisTransform;
            this.throwUpForce = throwUpForce;
            this.throwForce = throwForce;
            this.firePoint = firePoint;
        }

        public override NodeState Evaluate()
        {
            // Någonting att slänga mot.
            // Någonitng att slänga.
            target = (Transform) GetData("target");

            Debug.Log(target);


            if (target == null)
            {
                return NodeState.FAILURE;
            }


            thisTransform.LookAt(target);

            if (timer <= 0)
            {
                ShootEventInfo rockThrow = new ShootEventInfo(firePoint, this.rockThrow, throwUpForce, throwForce);
                EventSystem.current.FireEvent(rockThrow);
                timer = cooldown;
            }

            timer -= Time.deltaTime;

            return NodeState.RUNNING;
        }
    }
}