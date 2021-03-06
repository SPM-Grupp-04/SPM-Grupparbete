using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class FollowPlayer :TreeNode
{
    private Transform droneTransform;
    private Transform playerTransform;
    public FollowPlayer(Transform drone, Transform player)
    {
        droneTransform = drone;
        playerTransform = player;


    }

    public override NodeState Evaluate()
    {

        // Follow Player Logic
        if (Vector3.Distance(droneTransform.position, playerTransform.position) > 2f)
        {
            droneTransform.position = Vector3.MoveTowards(droneTransform.position, playerTransform.position, 
                DroneBT.droneSpeed * Time.deltaTime );
            
            droneTransform.LookAt(playerTransform.position);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
