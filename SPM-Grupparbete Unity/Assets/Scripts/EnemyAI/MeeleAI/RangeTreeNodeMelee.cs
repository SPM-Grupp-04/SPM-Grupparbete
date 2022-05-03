using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class RangeTreeNodeMelee : TreeNode
{
    
    /*
     * Klassen kollar om ett target finns inom det in skickade värdet. 
     */
    private float range;
   // private Transform target;
  // private List<Transform> targets;
    //private Transform orgin;
    private const int largeDistanceNumber = 100;
    private Animator _animator;
    private Vector3 target;
    private float distanceToPlayer;
    public RangeTreeNodeMelee(float range,Vector3 targetPos, float distanceToPlayer , Animator animator)
    {
        this.range = range;
      //  this.targets = targets;
        //this.orgin = orgin;
        _animator = animator;
        target = targetPos;
        this.distanceToPlayer = distanceToPlayer;
    }

    public override NodeState Evaluate()
    {
        /*float distance = largeDistanceNumber;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, orgin.position);
            Debug.Log(target + "RangeNode");
            if (tempdistance < distance && target.gameObject.activeInHierarchy)
            {
                distance = tempdistance;
               
                this.target = target;
            }

                
        }*/
        
        state = distanceToPlayer <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        if (state == NodeState.FAILURE)
        {
            _animator.SetBool("Run",false);
        }
        return state;
    }
}