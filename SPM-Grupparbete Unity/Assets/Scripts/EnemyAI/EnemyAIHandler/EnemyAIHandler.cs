using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
using Tree = BehaviorTree.Tree;


public class EnemyAIHandler : MonoBehaviour
{
    // Ref till spelarna:
    //[SerializeField] private GameObject targetOne;
    //[SerializeField] private GameObject targetTwo;

    public List<BaseClassEnemyAI> units = new List<BaseClassEnemyAI>();
    private GameObject playerOne;
    private GameObject playerTwo;
    private static Vector3 dynamite;
    private float distanceToDynamite;

    [SerializeField] [Range(0.1f, 5f)] private float raidusAroundTarget = 0.5f;

    private void Start()
    {
       
            playerOne = GameObject.Find("Player1");
            Debug.Log(playerOne);
            playerTwo = GameObject.Find("Player2");
           Debug.Log(playerTwo);
            
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
        
        
        Vector3 playerOnePosition = playerOne.transform.position;
        Vector3 playerTowPosition = playerTwo.transform.position;
        bool playerOneIsActive = playerOne.gameObject.activeInHierarchy;
        bool playerTwoIsActive = playerTwo.gameObject.activeInHierarchy;
        int countEnemy = 0;
        foreach (BaseClassEnemyAI enemyAI in units)
        {
            if (enemyAI.gameObject.activeInHierarchy)
            {
                // Det är är din distanc till spelaren 
                // Här är nummret på din plats i cirkeln som ska omringa spelaren
                // EnemyAI.SetDistance till spelaren
                // EnemyAI.SetDinPlats i cirkel
                Vector3 aiPos = enemyAI.gameObject.transform.position;


                float distancePlayerOne = Vector3.Distance(playerOnePosition, aiPos);
                float distancePlayerTwo = Vector3.Distance(playerTowPosition, aiPos);
                if (dynamite != Vector3.zero)
                {
                    distanceToDynamite = Vector3.Distance(dynamite, aiPos);
                }


                Vector3 closestTarget = new Vector3();
                float closestDistance = 100;

                if (!playerOneIsActive)
                {
                    distancePlayerOne = int.MaxValue;
                }

                if (!playerTwoIsActive)
                {
                    distancePlayerTwo = int.MaxValue;
                }


                if (dynamite != Vector3.zero && enemyAI.randomNumber >= 6)
                {
                    closestTarget = CalulatePosInCirkel(dynamite, countEnemy);
                    closestDistance = distanceToDynamite;
                }
                else if (distancePlayerOne < distancePlayerTwo && playerOneIsActive)
                {
                    enemyAI.PlayerPos(playerOnePosition);
                    closestTarget = CalulatePosInCirkel(playerOnePosition, countEnemy);
                    closestDistance = distancePlayerOne;
                }
                else if (playerTwoIsActive)
                {
                    enemyAI.PlayerPos(playerTowPosition);
                    closestTarget = CalulatePosInCirkel(playerTowPosition, countEnemy);
                    closestDistance = distancePlayerTwo;
                }

                enemyAI.PositionAroundTarget(closestTarget);
                enemyAI.DistanceToPlayerPos(closestDistance);


                enemyAI.m_TopTreeNode.Evaluate();
                countEnemy++;
            }
        }
    }


    private Vector3 CalulatePosInCirkel(Vector3 playerPos, int countEnemy)
    {
        return new Vector3(playerPos.x + raidusAroundTarget *
            Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count),
            playerPos.y,
            playerPos.z + raidusAroundTarget *
            Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count));
    }
}