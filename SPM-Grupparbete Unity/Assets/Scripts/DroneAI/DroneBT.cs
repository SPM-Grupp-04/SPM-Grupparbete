using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class DroneBT : Tree
{
    //Grund klassen för en drönare. Hör byggs trädet upp och variablar som alla drönare behöver läggstill.
    public static float fov = 6f;
    public static float fovAttackRange = 1.5f;
    public static float droneSpeed = 4;

    [SerializeField] private Transform playerTransform;

    protected override TreeNode SetUpTree()
    {

     TreeNode root = new Selector(new List<TreeNode> // Sätter roten till den sequens som är korrekt eller till att man följer spelaren.
        {
            new Sequence(new List<TreeNode> 
            {
                
                new CheckEnemyInAttackRange(transform),
                new AttackEnemy(transform), 
                
            }),
            
            new Sequence(new List<TreeNode> 
            {
                new CheckIfEnemyIsInRange(transform),
                new GoToEnemy(transform), 
                
            }),
            
            
            new FollowPlayer(transform, playerTransform), // Fall back Action.
        });
        

        return root;
    }
}