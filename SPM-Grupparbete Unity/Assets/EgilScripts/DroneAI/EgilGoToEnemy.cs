using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using BehaviorTree;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class EgilGoToEnemy : EgilNode
{
    private Transform _transform;

    public EgilGoToEnemy(Transform transform)
    {
        _transform = transform;
    }
    
    public override NodeState Evaluate()
    {
        Transform target = (Transform) GetData("target");
        if (Vector3.Distance(_transform.position, target.position) > 2f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position,
                EgilDroneBT.droneSpeed * Time.deltaTime);
            
            _transform.LookAt(target.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}