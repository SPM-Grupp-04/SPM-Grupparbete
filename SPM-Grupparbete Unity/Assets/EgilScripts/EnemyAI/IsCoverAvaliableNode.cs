using BehaviorTree;
using UnityEngine;


public class IsCoverAvaliableNode : EgilNode
{
    private Cover[] avaliabbaleCover;
    private Transform playerTransorm;
    private EnemyAI ai;


    public IsCoverAvaliableNode(Cover[] avaliabbaleCover, Transform playerTransorm, EnemyAI ai)
    {
        this.avaliabbaleCover = avaliabbaleCover;
        this.playerTransorm = playerTransorm;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform bestSpot = FindBestCoverSpot();
        ai.SetBestCover(bestSpot);
        
        state = bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;

        return state;
    }

    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestSpot = null;

        for (int i = 0; i < avaliabbaleCover.Length; i++)
        {
            Transform bestSpotInCover = FindBestInCoverSpot(avaliabbaleCover[i], ref minAngle);
            if (bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
            }
        }

        return bestSpot;
    }

    private Transform FindBestInCoverSpot(Cover cover, ref float minAngle)
    {
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = playerTransorm.position - avaliableSpots[i].position;
            if (CheckIfSpotIsValid(avaliableSpots[i]))
            {
                float angle = Vector3.Angle(avaliableSpots[i].forward, direction);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = avaliableSpots[i];
                }
            }
        }

        return bestSpot;
    }

    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 direction = playerTransorm.position - spot.position;
        if (Physics.Raycast(spot.position, direction, out hit))
        {
            if (hit.collider.transform != playerTransorm)
            {
                return true;
            }
        }

        return false;
    }
}