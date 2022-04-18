using BehaviorTree;
using UnityEngine;

    public class IsCoverdNode : EgilNode
    {
        private Transform target;
        private Transform[] targets;
        private Transform origin;

        public IsCoverdNode(Transform[] targets, Transform origin)
        {
            this.targets = targets;
            this.origin = origin;
        }

        public override NodeState Evaluate()
        {
            foreach (Transform target in targets)
            {
                this.target = target;
            }
            
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
