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
        //ClearData("Target");
        // Follow Player Logic
        if (Vector3.Distance(droneTransform.position, playerTransform.position) > 3f)
        {
            droneTransform.position = Vector3.MoveTowards(droneTransform.position, 
                new Vector3(playerTransform.position.x, 2, playerTransform.transform.position.z), 
                DroneBT.droneSpeed * Time.deltaTime );
            
            droneTransform.LookAt(playerTransform.position);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
