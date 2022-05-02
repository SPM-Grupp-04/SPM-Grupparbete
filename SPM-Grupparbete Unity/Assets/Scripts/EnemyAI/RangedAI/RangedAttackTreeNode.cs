using System.Collections.Generic;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;
using Event = UnityEngine.Event;

namespace Utility.EnemyAI
{
    public class RangedAttackTreeNode : TreeNode
    {
        private NavMeshAgent agent;
        private GameObject gameObject;
        private Transform target;
        private List<Transform> targets;
        private GameObject throwableObject;
        private Vector3 currentVelocity;
        private float smoothDamp;
        private const int largeDistanceNumber = 100;

        [Header("ThrowSettings")] 
        private float throwCD;
        private float throwUpForce;
        private float throwForce;
        private float timer;
        
  

        public RangedAttackTreeNode(NavMeshAgent agent, GameObject gameObject, List<Transform> targets,
            GameObject throwabelObject, float throwCD, float throwUpForce, float throwForce)
        {
            this.agent = agent;
            this.gameObject = gameObject;
            this.targets = targets;
            this.throwableObject = throwabelObject;
            smoothDamp = 1f;
            this.throwCD = throwCD;
            this.throwUpForce = throwUpForce;
            this.throwForce = throwForce;
            timer = throwCD;
        }

        public override NodeState Evaluate()
        {
            float distance = largeDistanceNumber;
            foreach (Transform target in targets)
            {
                float tempdistance = Vector3.Distance(target.position, agent.transform.position);

                if (tempdistance < distance)
                {
                    distance = tempdistance;
                    this.target = target;
                }
            }

            agent.isStopped = true;

            Vector3 direction = target.position - gameObject.transform.position;
            Vector3 currentDirection =
                Vector3.SmoothDamp(gameObject.transform.forward, direction, ref currentVelocity, smoothDamp);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            gameObject.transform.rotation = rotation;


            var shootEventInfo = new ShootEventInfo(this.gameObject,
                this.throwableObject, throwUpForce, throwForce);

            if (timer > throwCD)
            {
                EventSystem.current.FireEvent(shootEventInfo);
                timer = 0;
            }

            timer += Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        }

       
    }
}