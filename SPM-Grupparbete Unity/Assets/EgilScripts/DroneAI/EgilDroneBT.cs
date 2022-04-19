using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EgilDroneBT : EgilTree
{
    //Grund klassen för en drönare. Hör byggs trädet upp och variablar som alla drönare behöver läggstill.
    public static float fov = 6f;
    public static float fovAttackRange = 1.5f;
    public static float droneSpeed = 4;

    [SerializeField] private Transform playerTransform;

    protected override EgilNode SetUpTree()
    {

     EgilNode root = new EgilSelector(new List<EgilNode> // Sätter roten till den sequens som är korrekt eller till att man följer spelaren.
        {
            new EgilSequence(new List<EgilNode> 
            {
                
                new CheckEnemyInAttackRange(transform),
                new EgilAttackEnemy(transform), 
                
            }),
            
            new EgilSequence(new List<EgilNode> 
            {
                new EgilCheckIfEnemyIsInRange(transform),
                new EgilGoToEnemy(transform), 
                
            }),
            
            
            new EgilFollowPlayer(transform, playerTransform), // Fall back Action.
        });
        

        return root;
    }
}