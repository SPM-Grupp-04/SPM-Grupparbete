using BehaviorTree;
using UnityEngine;

    public class IsCoverdNode : EgilNode
    {
        private Transform target;
        private Transform origin;

        public IsCoverdNode(Transform target, Transform origin)
        {
            this.target = target;
            this.origin = origin;
        }

        public override NodeState Evaluate()
        {
            RaycastHit hit;
            if(Physics.Raycast(origin.position, target.position - origin.position, out hit))
            {
                if(hit.collider.transform != target)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }   
        
        
    }
