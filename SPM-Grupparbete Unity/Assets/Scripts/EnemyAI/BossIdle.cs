using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;


namespace EnemyAI
{
    public class BossIdle : TreeNode
    {
        private NavMeshAgent agent;
        private Animator animator;
        private List<Transform> wayPoints;

        public BossIdle(NavMeshAgent agent, Animator animator,
            List<Transform> wayPoints)
        {
            this.agent = agent;
            this.animator = animator;
            this.wayPoints = wayPoints;
        }

        private float timer = 5;

        public override NodeState Evaluate()
        {
            int randomPoint = UnityEngine.Random.Range(0, wayPoints.Count);

            if (timer >= 5)
            {
                agent.SetDestination(new Vector3(wayPoints[randomPoint].position.x, agent.transform.position.y,
                    wayPoints[randomPoint].position.x));
                
                timer = 0;
            }

            timer += Time.deltaTime;

            agent.speed = 3;
            animator.SetBool("Run", true);


            return NodeState.SUCCESS;
        }
    }
}