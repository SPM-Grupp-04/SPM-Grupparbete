using BehaviorTree;
using UnityEngine;
using UnityEngine.Pool;
using Tree = BehaviorTree.Tree;

namespace EnemyAI
{
    public abstract class BaseEnemyAI : Tree
    {
        /*protected IObjectPool<BaseClassEnemyAI> pool;*/
        public TreeNode TopTreeNode;
    
        [HideInInspector]
        public int randomNumber; // Gets used in EnemyAIHandler therefor needs to be public
   
        public float movementSpeed;
        public float currentHealth;
        public float startingHealth;
        protected Animator animator;
    
    
        public abstract void SetPool(IObjectPool<BaseEnemyAI> pool);
        public abstract void PositionAroundTarget(Vector3 targetPos);
        public abstract void DistanceToPlayerPos(float distance);
        public abstract void PlayerPos(Vector3 playerPos);
        public abstract float GetCurrentHealth();
    }
}
