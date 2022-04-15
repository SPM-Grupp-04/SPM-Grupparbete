using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class EgilFollowPlayer :EgilNode
{
    private Transform droneTransform;
    private Transform playerTransform;
    public EgilFollowPlayer(Transform drone, Transform player)
    {
        droneTransform = drone;
        playerTransform = player;


    }

    public override NodeState Evaluate()
    {

        // Follow Player Logic
        if (Vector3.Distance(droneTransform.position, playerTransform.position) > 2f)
        {
            droneTransform.position = Vector3.MoveTowards(droneTransform.position, playerTransform.position, 4f * Time.deltaTime );
            droneTransform.LookAt(playerTransform.position);
        }
        
        Debug.Log("Following Player");

        state = NodeState.RUNNING;
        return state;
    }
}
