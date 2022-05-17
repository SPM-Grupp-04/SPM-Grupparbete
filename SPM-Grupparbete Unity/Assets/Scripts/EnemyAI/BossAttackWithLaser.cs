using UnityEngine;
using BehaviorTree;
using EgilEventSystem;
using EgilScripts.DieEvents;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class BossAttackWithLaser : TreeNode
{
    private NavMeshAgent agent;
    private Transform firePoint;
    private Transform firePointTwo;
    private Animator _animator;
    private LineRenderer lineRenderer;
    private LineRenderer lineTwo;
    private float attackRange;
    private float overHeatTime = 5;

    public BossAttackWithLaser(NavMeshAgent agent, LineRenderer lineRenderer, LineRenderer lineTwo,
        Transform firePoint, Transform firePointTwo, float fov, Animator animator)
    {
        this.lineTwo = lineTwo;
        this.agent = agent;
        this.firePointTwo = firePointTwo;
        this.firePoint = firePoint;
        this._animator = animator;
        this.lineRenderer = lineRenderer;


        attackRange = fov;
    }

    private bool isOverHeated = false;
    private Transform target;
    private float timer = 0;

    public override NodeState Evaluate()
    {
        if (timer > overHeatTime)
        {
            isOverHeated = true;
        }

        if (timer <= 0)
        {
            isOverHeated = false;
        }

        if (isOverHeated)
        {
            timer -= Time.deltaTime;
            lineRenderer.enabled = false;
            lineTwo.enabled = false;
            ClearData("target");
            return NodeState.FAILURE;
        }

        if (!isOverHeated)
        {
            timer += Time.deltaTime;
        }

        target = (Transform) GetData("target");

        if (target == null || !target.gameObject.activeInHierarchy ||
            Vector3.Distance(agent.transform.position, target.position) > attackRange)
        {
            lineRenderer.enabled = false;
            lineTwo.enabled = false;
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        _animator.SetBool("Run", false);
        LockOnTarget();
        
        if (target.gameObject.layer == 6)
        {
            Laser();
        }

        return NodeState.RUNNING;
    }


    void Laser()
    {
        DealDamageEventInfo dealDamageEventInfo = new DealDamageEventInfo(target.gameObject, 4 * Time.deltaTime);
        EventSystem.current.FireEvent(dealDamageEventInfo);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            lineTwo.enabled = true;
        }

        Vector3 lineHitPoint = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, lineHitPoint);

        lineTwo.SetPosition(0, firePointTwo.position);
        lineTwo.SetPosition(1, lineHitPoint);
    }

    private bool canShoot;

    void LockOnTarget()
    {
        agent.transform.LookAt(target);
        agent.isStopped = true;
      
    }
}