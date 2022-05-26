using BehaviorTree;
using UnityEngine;

namespace EnemyAI
{
    public class CheckPlayerInAttackRange : TreeNode
    {
        private readonly Transform agentsTransform;
        private readonly LayerMask layerMask = LayerMask.GetMask("Player");
        private readonly float fov;
        private BossAI boss;
        public CheckPlayerInAttackRange(BossAI boss,Transform agentsTransform, float fov)
        {
            this.agentsTransform = agentsTransform;
            this.fov = fov;
            this.boss = boss;
        }

        public override NodeState Evaluate()
        {

       
            object t = GetData("target");
        
            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    agentsTransform.position, fov, layerMask);
            
                if (colliders.Length > 0)
                {
                    parent.parent.SetData("target", colliders[0].transform);
                    return NodeState.SUCCESS;
                }
                ClearData("target");
                return NodeState.FAILURE;
            }
    
            Transform target = (Transform) GetData("target");
            float distance = Vector3.Distance(agentsTransform.position, target.position);
      
            if (distance > fov)
            {
                ClearData("target");
                return NodeState.FAILURE;
            }
        
            state = NodeState.SUCCESS;
            return state;
        }
    }
}