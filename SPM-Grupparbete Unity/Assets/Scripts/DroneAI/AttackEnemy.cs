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
    private LineRenderer lr;
    private LayerMask ignorMask;

    public AttackEnemy(Transform transform, LayerMask ignorMask, LineRenderer lr)
    {
        timeRemaining = cooldownTime;
        _transform = transform;
        this.lr = lr;
        this.ignorMask = ignorMask;
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform) GetData("target");
        Debug.Log("Drone Target " + target);

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
            if (target != null)
            {
                ShootObject(target);
                
            }
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

    void LaserBetweenPoints(Vector3 start, Vector3 end)
    {
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }


    private void ShootObject(Transform target)
    {
        Vector3 transformPos = _transform.position;

        RaycastHit shootHit;
        // Vector3 fwd = target.TransformDirection(Vector3.forward);


        if (Physics.Raycast(transformPos, target.position, out shootHit, 10f, ignorMask))
        {
            //Debug.DrawLine(transformPos, shootHit.point, Color.green);
            if (shootHit.collider.gameObject.CompareTag("Enemy"))
            {
                LaserBetweenPoints(transformPos, shootHit.point);
                Debug.Log("Enemy getting hit");
                //shootHit.collider.gameObject.SendMessage("TakeDamage");
                var takeDamge = new DealDamageEventInfo(shootHit.collider.gameObject, 1);
                EventSystem.current.FireEvent(takeDamge);
            }

            return;
        }
    }
}