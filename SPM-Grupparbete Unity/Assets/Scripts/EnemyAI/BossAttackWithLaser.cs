using UnityEngine;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine.InputSystem;


public class BossAttackWithLaser : TreeNode
{
    private Transform _transform;
    private Transform firePoint;
    private Transform firePointTwo;
    private Animator _animator;
    private LineRenderer _lineRenderer;
    private LineRenderer _lineTwo;
    private float attackRange;

    public BossAttackWithLaser(Transform transform, LineRenderer lineRenderer,LineRenderer lineTwo,
        Transform firePoint,Transform firePointTwo ,float fov, Animator animator)
    {
        this._lineTwo = lineTwo;
        _transform = transform;
        this.firePointTwo = firePointTwo;
        this.firePoint = firePoint;
        this._animator = animator;
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
            _lineTwo.enabled = false;
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }
        _animator.SetBool("Run", false);
        LockOnTarget();


       if (target.gameObject.layer == 6  )
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
        DamageDealt dealDamageEventInfo = new DamageDealt(target.gameObject, 4 * Time.deltaTime);
        EventSystem.current.FireEvent(dealDamageEventInfo);
        if (!_lineRenderer.enabled)
        {
            _lineRenderer.enabled = true;
            _lineTwo.enabled = true;
        }

        Vector3 lineHitPoint=    new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);
        _lineRenderer.SetPosition(0, firePoint.position);
        _lineRenderer.SetPosition(1, lineHitPoint);
       
        _lineTwo.SetPosition(0, firePointTwo.position);
        _lineTwo.SetPosition(1, lineHitPoint);
    }

    private bool canShoot;
    void LockOnTarget()
    {
        
        _transform.LookAt(target);
        /*
        Vector3 dir = target.position - _transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        
        Vector3 rotation = Quaternion.Lerp(partToRotate.localRotation, lookRotation, Time.deltaTime * 3 /*TurnSpeed#1#)
            .eulerAngles;

        var hit = Physics.Linecast(partToRotate.position,new Vector3(target.position.x,
                target.position.y + 0.5f, target.position.z)
            , 7);
        
        

        if (hit)
        {
            Debug.Log("hit");
            canShoot = false;
            _lineRenderer.enabled = false;
        }
        else
        {
            canShoot = true;
        }
        
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        */
        
        
        
    }
}