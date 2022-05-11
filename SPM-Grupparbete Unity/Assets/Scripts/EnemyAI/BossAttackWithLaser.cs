using UnityEngine;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using Unity.Mathematics;
using UnityEngine.InputSystem;


public class BossAttackWithLaser : TreeNode
{
    private Transform _transform;
    private Transform firePoint;
    private Transform partToRotate;
    private LineRenderer _lineRenderer;
    private float attackRange;

    public BossAttackWithLaser(Transform transform, Transform partToRotate, LineRenderer lineRenderer,
        Transform firePoint, float fov)
    {
        _transform = transform;
        this.firePoint = firePoint;
        this.partToRotate = partToRotate;
        this._lineRenderer = lineRenderer;

        attackRange = fov;
    }

    private Transform target;

    public override NodeState Evaluate()
    {
        target = (Transform) GetData("target");


        if (target == null || !target.gameObject.activeInHierarchy ||
            Vector3.Distance(_transform.position, target.position) > attackRange)
        {
            _lineRenderer.enabled = false;
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        LockOnTarget();


       if (target.gameObject.layer == 6 && canShoot )
        {
            Laser();
        }


        state = NodeState.RUNNING;
        cooldown += Time.deltaTime;
        return state;
    }

    private float cooldown = 0.5f;


    void Laser()
    {
        DealDamageEventInfo dealDamageEventInfo = new DealDamageEventInfo(target.gameObject, 4 * Time.deltaTime);
        EventSystem.current.FireEvent(dealDamageEventInfo);
        if (!_lineRenderer.enabled)
        {
            _lineRenderer.enabled = true;
        }

        _lineRenderer.SetPosition(0, firePoint.position);
        _lineRenderer.SetPosition(1, new Vector3(target.position.x, target.position.y + 0.5f, target.position.z));
    }

    private bool canShoot;
    void LockOnTarget()
    {
        Vector3 dir = target.position - _transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 3 /*TurnSpeed*/)
            .eulerAngles;

        Debug.Log(rotation);
        if (rotation.y < 160 || rotation.y > 350)
        {
            canShoot = false;
            _lineRenderer.enabled = false;
        }
        else
        {
            canShoot = true;
        }
        
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        
        
        
    }
}