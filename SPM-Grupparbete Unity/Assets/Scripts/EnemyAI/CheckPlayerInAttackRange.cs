using BehaviorTree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class CheckPlayerInAttackRange : TreeNode
{
    private Transform objectItsAttachedToTransform;
    private LayerMask layerMask = LayerMask.GetMask("Player");
    private float fov;
    public CheckPlayerInAttackRange(Transform objectItsAttachedToTransform, float fov)
    {
        this.objectItsAttachedToTransform = objectItsAttachedToTransform;
        this.fov = fov;

    }

    public override NodeState Evaluate()
    {
        // Kollar om det finns någont target i Mappen.
        object t = GetData("target");

        // Om det inte finns så gör vi en overlapsphare cast som kollar om det finns någon fiende i närheten
        // Vi träffar bara sådana som har en layermask Enemy(7) på sig.
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                objectItsAttachedToTransform.position, fov, layerMask);

            // OM vi träffade någonting så ska vi sätta det på collider plats 0. med dess trannsform som value.
            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                state = NodeState.SUCCESS;

                return state;
            }

            // Det fanns ingen inärheten och vi får fail i sate.
            ;
            state = NodeState.FAILURE;
            return state;
        }

        // är inte null vilket betyder att vi redan har lagt till en i mappen
        // och inte tagit bort den än( detta innebär att den fortfarande är nära eller inte dött.
        state = NodeState.SUCCESS;
        return state;
    }
}