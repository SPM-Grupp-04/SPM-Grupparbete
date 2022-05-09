using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEditor.Timeline.Actions;

public class FollowPlayer : TreeNode
{
    private Transform droneTransform;
    private Transform playerTransform;
    private int resAmount = 1;
    private Animator _animator;
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
                DroneBT.droneSpeed * Time.deltaTime);

            droneTransform.LookAt(playerTransform.position);
        }

        if (!playerTransform.gameObject.activeInHierarchy && resAmount > 0  && Vector3.Distance(droneTransform.position, playerTransform.position) < 5)
        {
            if (playerTransform.gameObject.name == "Player1")
            {
                PlayerStatistics.Instance.playerOneHealth = PlayerStatistics.Instance.playerMaxHealth / 2;
            }
            else
            {
                PlayerStatistics.Instance.playerTwoHealth = PlayerStatistics.Instance.playerMaxHealth / 2;
            }
            playerTransform.gameObject.SetActive(true);
            resAmount--;
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}