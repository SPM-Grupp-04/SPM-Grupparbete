using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;


public class EnemyAIHandler : MonoBehaviour
{
    // Ref till spelarna:
    //[SerializeField] private GameObject targetOne;
    //[SerializeField] private GameObject targetTwo;

    public List<BaseClassEnemyAI> units = new List<BaseClassEnemyAI>();
    private Transform playerOne;
    private Transform playerTwo;

    [SerializeField] [Range(0.1f, 5f)] private float raidusAroundTarget = 0.5f;

    private void Start()
    {
        playerOne = GameObject.Find("Player1").transform;
        playerTwo = GameObject.Find("Player2").transform;
    }

    private void Update()
    {
        Vector3 playerOnePosition = playerOne.position;
        Vector3 playerTowPosition = playerTwo.position;
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
                
                if (distancePlayerOne < distancePlayerTwo && playerOneIsActive)
                {
                    enemyAI.PlayerPos(playerOnePosition);
                    closestTarget = new Vector3(playerOnePosition.x + raidusAroundTarget *
                        Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count),
                        playerOnePosition.y,
                        playerOnePosition.z + raidusAroundTarget *
                        Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count));
                    /*closestTarget = playerOnePosition;*/
                    closestDistance = distancePlayerOne;
                }
                else if (playerTwoIsActive)
                {
                    enemyAI.PlayerPos(playerTowPosition);
                    closestTarget = new Vector3(playerTowPosition.x + raidusAroundTarget *
                        Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count),
                        playerTowPosition.y,
                        playerTowPosition.z + raidusAroundTarget *
                        Mathf.Cos(2 * Mathf.PI * countEnemy / units.Count));
                    /*closestTarget = playerTowPosition;*/
                    closestDistance = distancePlayerTwo;
                }

                enemyAI.PositionAroundTarget(closestTarget);
                enemyAI.DistanceToPlayerPos(closestDistance);
              


                enemyAI.m_TopTreeNode.Evaluate();
                countEnemy++;
            }
        }
    }
}