using System.Collections.Generic;
using System.Diagnostics;
using BehaviorTree;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;


public class MeeleAttackTreeNode : TreeNode
{
    /*
     * Klassen gör så att fienden vänder sig mot sitt mål och sedan attackerar det. Här väljer den också
     * Vilket mål den ska attackera
     * 
     */
    private NavMeshAgent agent;

    private GameObject gameObject;

    // private Transform target;
    private List<Transform> targets;
    private Vector3 currentVelocity;
    private float smoothDamp;
    private Animator _animator;
    private MeleeWepon _meleeWepon;
    private Vector3 target;


    public MeeleAttackTreeNode(Vector3 target, NavMeshAgent agent, Animator animator, MeleeWepon meleeWepon)
    {
        this.agent = agent;
        //  this.gameObject = gameObject;

        this.target = target;

        smoothDamp = 1f;
        _animator = animator;
        _meleeWepon = meleeWepon;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        Transform agentT = agent.transform;
        //ai.SetColor(Color.red);
        // Vector3 direction = target - agentT.position;
        //  Vector3 currentDirection = Vector3.SmoothDamp(agentT.forward, target, ref currentVelocity, smoothDamp);
        //   Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        //  agentT.rotation = rotation;

        agentT.LookAt(target);

        // TODO: Ändra senare.!!!!!!!
        if (_meleeWepon.timeRemaining <= 0.1f)
        {
            _animator.SetTrigger("Attack");
        }

        state = NodeState.RUNNING;
        return state;
    }
}