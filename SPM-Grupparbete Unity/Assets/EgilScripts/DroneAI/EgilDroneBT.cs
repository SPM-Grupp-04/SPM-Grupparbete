using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EgilDroneBT : EgilTree
{
    public static float fov = 6f;
    public static float fovAttackRange = 3f;

    [SerializeField] private Transform playerTransform;

    protected override EgilNode SetUpTree()
    {
       //' EgilNode root = new EgilFollowPlayer(transform,playerTransform);
     // Node root = new CheckIfEnemyIsInRange(transform);
     //Node root = new GoToEnemy(transform);
    
     EgilNode root = new EgilSelector(new List<EgilNode> //  Rooten är antingen en av CheckIfEnemyIsInRange, GoToEnemy eller Follow player.
        {
            new EgilSequence(new List<EgilNode> 
            {
                
                new CheckEnemyInAttackRange(transform),
                new EgilAttackEnemy(transform), 
                
            }),
            
            new EgilSequence(new List<EgilNode> // Blir true om alla state i sig är sanna.
            {
                new EgilCheckIfEnemyIsInRange(transform),
                new EgilGoToEnemy(transform), // Skapar Null BuT WHY?
                
            }),
            
            
            new EgilFollowPlayer(transform, playerTransform), // Fall back Action.
        });
        

        return root;
    }
}