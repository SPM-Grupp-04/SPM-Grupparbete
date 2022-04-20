using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class HealthNode : EgilNode
{
    private EnemyAI ai;
    private float threshold;

    public HealthNode(EnemyAI ai, float threshold)
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
