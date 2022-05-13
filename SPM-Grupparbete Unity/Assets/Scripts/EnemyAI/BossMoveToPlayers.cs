using BehaviorTree;
using UnityEngine;

public class BossMoveToPlayers : TreeNode
{
    private Transform bossTransfrom;
    private Transform target;
    private Animator animator;
    private int moveSpeed = 3;

    public BossMoveToPlayers(Transform bossTransfrom, Animator animator)
    {
        this.bossTransfrom = bossTransfrom;
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        
        if (t != null)
        {
            animator.SetBool("Run", true);
            target = (Transform) GetData("target");
            
            bossTransfrom.position = Vector3.MoveTowards(bossTransfrom.position, target.transform.position, 3 * Time.deltaTime);
            bossTransfrom.transform.LookAt(target);
            return NodeState.RUNNING;
        }
        
        animator.SetBool("Run", false);
        ClearData("target");

        return NodeState.FAILURE;
    }
}