using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace Utility.EnemyAI
{
    public class RangedAttackTreeNode : TreeNode
    {
        
        private NavMeshAgent agent;
        private GameObject gameObject;
        private Transform target;
        private Transform[] targets;

        private Vector3 currentVelocity;
        private float smoothDamp;
        private const int largeDistanceNumber = 100;
    
    
        public RangedAttackTreeNode(NavMeshAgent agent, GameObject gameObject, Transform[] targets)
        {
            this.agent = agent;
            this.gameObject = gameObject;
            this.targets = targets;
            smoothDamp = 1f;
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
            Vector3 currentDirection = Vector3.SmoothDamp(gameObject.transform.forward, direction, ref currentVelocity, smoothDamp);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            gameObject.transform.rotation = rotation;
            
            
            // Attack 
       
            state = NodeState.RUNNING;
            return state;
        }
        
    }
}