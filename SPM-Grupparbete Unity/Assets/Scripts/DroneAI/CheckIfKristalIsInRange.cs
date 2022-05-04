using BehaviorTree;
using UnityEngine;

namespace Utility.DroneAI
{
    public class CheckIfKristalIsInRange : TreeNode
    {
        
        
        public override NodeState Evaluate()
        {

            // Om Kristaller finns i närheten.
            // gå dit.
            
            /*object t = GetData("target");

            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    _transform.position, DroneBT.fov,enemyLayerMask);

                if (colliders.Length > 0)
                {
                    parent.parent.SetData("target",colliders[0].transform);
                    state = NodeState.SUCCESS;
                
                    return state;
                }
            
                state = NodeState.FAILURE;
                return state;
            }*/
            
            return NodeState.FAILURE;
        }
    }
}