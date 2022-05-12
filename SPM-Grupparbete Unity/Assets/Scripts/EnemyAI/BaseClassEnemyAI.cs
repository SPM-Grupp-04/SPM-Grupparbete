using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;

public abstract class BaseClassEnemyAI : Tree
{
    /*protected IObjectPool<BaseClassEnemyAI> pool;*/
    public TreeNode m_TopTreeNode;

    public int randomNumber;

    public abstract void SetPool(IObjectPool<BaseClassEnemyAI> pool);
    public abstract void PositionAroundTarget(Vector3 TargetPos);
    public abstract void DistanceToPlayerPos(float distance);
    public abstract void PlayerPos(Vector3 playerPos);
}
