
using System.Collections.Generic;
using UnityEngine;



public class EnemyAIHandler : MonoBehaviour
{
    public List<BaseClassEnemyAI> units = new List<BaseClassEnemyAI>();
    
    private GameObject playerOne;
    private GameObject playerTwo;
    
    private static Vector3 dynamite;
    private Vector3 playerOnePosition;
    private Vector3 playerTowPosition;
    private Vector3 closestTarget;

    private bool playerOneIsActive;
    private bool playerTwoIsActive;

    private const float RadiusAroundTarget = 0.5f;
  
    private float distanceToDynamite;
    private float closestDistance;
    private float distancePlayerOne;
    private float distancePlayerTwo;


    private void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");

        //If a player died treat it as the other one. 
        if (playerOne == null)
        {
            playerOne = playerTwo;
        }

        if (playerTwo == null)
        {
            playerTwo = playerOne;
        }
    }

    public static void SetDynamite(Vector3 dyn)
    {
        dynamite = dyn;
    }


    private void Update()
    {
        playerOneIsActive = playerOne.gameObject.activeInHierarchy;
        playerTwoIsActive = playerTwo.gameObject.activeInHierarchy;

        // Getting the player position once.
        if (playerOneIsActive)
        {
            playerOnePosition = playerOne.transform.position;
        }

        if (playerTwoIsActive)
        {
            playerTowPosition = playerTwo.transform.position;
        }

        // Used to know were to position in the circle around tha player.
        var countEnemy = 0;

        foreach (BaseClassEnemyAI enemyAI in units)
        {
            if (enemyAI.gameObject.activeInHierarchy )
            {
                Vector3 aiPos = enemyAI.gameObject.transform.position;

                
                // Checking distance from Ai-unit to Player.
                distancePlayerOne = Vector3.Distance(playerOnePosition, aiPos);
                distancePlayerTwo = Vector3.Distance(playerTowPosition, aiPos);

                // Resetting the old values.
                closestTarget = new Vector3();
                closestDistance = 100;

                // A check needed to make sure the AI don't find the transform for the inactivated Game object.
                if (!playerOneIsActive)
                {
                    distancePlayerOne = 1000;
                }

                if (!playerTwoIsActive)
                {
                    distancePlayerTwo = 1000;
                }


                closestDistance = ClosestDistance(enemyAI, aiPos, countEnemy,
                    closestDistance, distancePlayerOne, distancePlayerTwo);

                enemyAI.PositionAroundTarget(closestTarget);
                enemyAI.DistanceToPlayerPos(closestDistance);
                
                enemyAI.MTopTreeNode.Evaluate();
                
                countEnemy++;
            }
           
        }
    }

    private float ClosestDistance(BaseClassEnemyAI enemyAI, Vector3 aiPos, int countEnemy, float closestDistance,
        float distancePlayerOne, float distancePlayerTwo)
    {
        if (dynamite != Vector3.zero && enemyAI.randomNumber >= 6)
        {
            // Checking the distance to the dynamite for every enemy that might need to know it.
            distanceToDynamite = Vector3.Distance(dynamite, aiPos);
            closestTarget = CalculatePositionInCircle(dynamite, countEnemy);
            closestDistance = distanceToDynamite;
        }
        else if (distancePlayerOne < distancePlayerTwo && playerOneIsActive)
        {
            enemyAI.PlayerPos(playerOnePosition);
            closestTarget = CalculatePositionInCircle(playerOnePosition, countEnemy);
            closestDistance = distancePlayerOne;
        }
        else if (playerTwoIsActive)
        {
            enemyAI.PlayerPos(playerTowPosition);
            closestTarget = CalculatePositionInCircle(playerTowPosition, countEnemy);
            closestDistance = distancePlayerTwo;
        }

        return closestDistance;
    }


    private Vector3 CalculatePositionInCircle(Vector3 playerPos, int countEnemy)
    {
        return new Vector3(playerPos.x + RadiusAroundTarget *
            Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count),
            playerPos.y,
            playerPos.z + RadiusAroundTarget *
            Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count));
    }
}