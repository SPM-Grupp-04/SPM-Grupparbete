using UnityEngine;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine.InputSystem;


public class AttackTarget : TreeNode
{
    private Transform transform;
    private Transform firePoint;
    private Transform partToRotate;
    private LineRenderer lineRenderer;
    private float attackRange;
    private const float Damage = 10;

    public AttackTarget(Transform transform, Transform partToRotate, LineRenderer lineRenderer, Transform firePoint, float fov)
    {
        this.transform = transform;
        this.firePoint = firePoint;
        this.partToRotate = partToRotate;
        this.lineRenderer = lineRenderer;

        attackRange = fov;
    }

    private Transform target;

    public override NodeState Evaluate()
    {
        target = (Transform) GetData("target");


        if (target == null || !target.gameObject.activeInHierarchy ||
            Vector3.Distance(transform.position, target.position) > attackRange)
        {
            lineRenderer.enabled = false;
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        LockOnTarget();

        if (target.gameObject.layer == 7)
        {
           
            Laser();
        }

        if (target.gameObject.layer == 12)
        {
           
            Drill();
        }


        state = NodeState.RUNNING;
        cooldown += Time.deltaTime;
        return state;
    }

    private float cooldown = 0.5f;

    void Drill()
    {
        if (cooldown > 0.5f)
        {
            target.gameObject.SendMessage("ReduceMaterialHP", 2);
            cooldown = 0;
        }

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, new Vector3(target.position.x, target.position.y + 0.5f, target.position.z));
    }


    void Laser()
    {
        DealDamageEventInfo dealDamageEventInfo = new DealDamageEventInfo(target.gameObject, Damage * Time.deltaTime);
        EventSystem.current.FireEvent(dealDamageEventInfo);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, new Vector3(target.position.x, target.position.y + 0.5f, target.position.z));
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 3 /*TurnSpeed*/)
            .eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}