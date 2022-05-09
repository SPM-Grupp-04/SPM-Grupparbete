using BehaviorTree;
using UnityEngine;

namespace Utility.DroneAI
{
    public class CheckIfKristalIsInRange : TreeNode
    {
        LayerMask ORE = LayerMask.GetMask("Ore");
        private Transform pos;
        private float fov;

        public CheckIfKristalIsInRange(Transform pos, float fov)
        {
            this.pos = pos;
            this.fov = fov;
        }
        public override NodeState Evaluate()
        {

            // Om Kristaller finns i närheten.
            // gå dit.
            
            
            
            object t = GetData("target");

      
            
            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    pos.position, fov, ORE);
                
                
                if (colliders.Length > 0)
                {
                    parent.parent.SetData("target",colliders[0].transform);
                    Debug.Log(colliders[0]);
                    state = NodeState.SUCCESS;
                
                    return state;
                }
            
                state = NodeState.FAILURE;
                return state;
            }
            
            return NodeState.FAILURE;
        }
    }
}