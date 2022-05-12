using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class HealthTreeNode : TreeNode
{
    private MeleeEnemyAI ai;
    private float threshold;

    public HealthTreeNode(MeleeEnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;

    }
    
    public override NodeState Evaluate()
    {
        state =  ai.CurrentHealth <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}
