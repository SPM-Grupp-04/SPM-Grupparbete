using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using BehaviorTree;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class GoToTarget : TreeNode
{
    private Transform _transform;

    public GoToTarget(Transform transform)
    {
        _transform = transform;
    }
    
    public override NodeState Evaluate()
    {
        Transform target = (Transform) GetData("target");
        if (Vector3.Distance(_transform.position, target.position) > 3f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(target.position.x, 2, target.position.z ),
                DroneBT.droneSpeed * Time.deltaTime);
            
            _transform.LookAt(target.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}