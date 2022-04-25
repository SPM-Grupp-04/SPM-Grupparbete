using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;

public abstract class BaseClassEnemyAI : Tree
{
    
    public abstract void SetPool(IObjectPool<BaseClassEnemyAI> pool);
   
}
