using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.Animations;

public class EgilDroneBT : EgilTree
{
    public static float fov = 6f;

    [SerializeField] private Transform playerTransform;

    protected override EgilNode SetUpTree()
    {
        EgilNode root = new EgilFollowPlayer(transform,playerTransform);
     // Node root = new CheckIfEnemyIsInRange(transform);
     //Node root = new GoToEnemy(transform);
    /* 
     Node root = new Selector(new List<Node> //  Rooten är antingen en av CheckIfEnemyIsInRange, GoToEnemy eller Follow player.
        {
            
            new Sequence(new List<Node> // Blir true om alla state i sig är sanna.
            {
                new CheckIfEnemyIsInRange(transform),
                new GoToEnemy(transform), // Skapar Null BuT WHY?
                
            }),
            
            
            new FollowPlayer(transform, playerTransform), // Fall back Action.
        });
        */

        return root;
    }
}