using BehaviorTree;
using UnityEngine;

namespace EnemyAI.MeeleAI
{
    public class RangeTreeNodeMelee : TreeNode
    {
        /*
     * Klassen kollar om ett target finns inom det in skickade värdet. 
     */
        private float range;
        // private Transform target;
        // private List<Transform> targets;
        //private Transform orgin;

        private Animator animator;

        private float distanceToPlayer;

        public RangeTreeNodeMelee(float range, float distanceToPlayer, Animator animator)
        {
            this.range = range;
            this.animator = animator;

            this.distanceToPlayer = distanceToPlayer;
        }

        public override NodeState Evaluate()
        {
       

            state = distanceToPlayer <= range ? NodeState.SUCCESS : NodeState.FAILURE;
           

            return state;
        }
    }
}