using BehaviorTree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


    public class CheckEnemyInAttackRange : EgilNode
    {

        private Transform _transform;

        public CheckEnemyInAttackRange(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("Target");

            if (t == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            var target = (Transform) t;

            if (Vector3.Distance(_transform.position, target.position) <= EgilDroneBT.fovAttackRange)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
