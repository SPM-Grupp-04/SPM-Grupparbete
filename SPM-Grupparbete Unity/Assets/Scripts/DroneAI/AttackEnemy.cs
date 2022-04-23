using UnityEngine;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine.InputSystem;

public class AttackEnemy : TreeNode
{
    private Transform _transform;
    private float cooldownTime = 0.5f;
    private float timeRemaining;

    public AttackEnemy(Transform transform)
    {
        timeRemaining = cooldownTime;
        _transform = transform;
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform) GetData("target");

        // Kollar om fienden har  dött.
        if (!target.gameObject.activeInHierarchy)
        {
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        // KOllar om fienden är för långt ifrån
        if (Vector3.Distance(_transform.position, target.position) > DroneBT.fovAttackRange)
        {
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        // Kör ett event när cooldownen tillåter.
        if (timeRemaining < 0.0f)
        {
            var damageEvent = new DealDamageEventInfo(target.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);
        }

        // OM cooldownen är klar reset.
        if (timeRemaining < 0.0f)
        {
            timeRemaining = cooldownTime;
        }

        // Minska tid.
        timeRemaining -= Time.deltaTime;
        state = NodeState.RUNNING;
        return state;
    }
}