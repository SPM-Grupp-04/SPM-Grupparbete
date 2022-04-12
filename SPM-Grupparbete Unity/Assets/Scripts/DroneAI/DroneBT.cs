using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.Animations;
using Tree = BehaviorTree.Tree;

public class DroneBT : Tree
{
    public static float fov = 6f;

    [SerializeField] private Transform playerTransform;

    protected override Node SetUpTree()
    {
        Node root = new FollowPlayer(transform,playerTransform);
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