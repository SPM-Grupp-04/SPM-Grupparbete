using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;

public abstract class BaseClassEnemyAI : Tree
{
    /*protected IObjectPool<BaseClassEnemyAI> pool;*/

    public abstract void SetPool(IObjectPool<BaseClassEnemyAI> pool);


}
