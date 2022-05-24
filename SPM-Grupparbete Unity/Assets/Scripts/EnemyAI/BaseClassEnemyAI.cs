using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;

public abstract class BaseClassEnemyAI : Tree
{
    /*protected IObjectPool<BaseClassEnemyAI> pool;*/
    public TreeNode MTopTreeNode;

    public int randomNumber;
    protected EgilEventSystem.EventSystem.EventListener damageDealtListener;

    public abstract void SetPool(IObjectPool<BaseClassEnemyAI> pool);
    public abstract void PositionAroundTarget(Vector3 targetPos);
    public abstract void DistanceToPlayerPos(float distance);
    public abstract void PlayerPos(Vector3 playerPos);
    public abstract float GetCurrentHealth();
}
