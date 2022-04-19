using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class EgilCheckIfEnemyIsInRange : EgilNode
{
    /*Klassen implementeras på liknande sätt som att kolla på om man är i attackrange bara det att man kollar med
     DroneBT.FOV Istället för DroneBT.AttackFOV.
     */
    public static int enemyLayerMask = 1 << 7;
    private Transform _transform;

    public EgilCheckIfEnemyIsInRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
            _transform.position, EgilDroneBT.fov,enemyLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target",colliders[0].transform);
                state = NodeState.SUCCESS;
                
                return state;
            }
            
            state = NodeState.FAILURE;
            return state;
        }
        

        state = NodeState.SUCCESS;
        return state;
    }
}
