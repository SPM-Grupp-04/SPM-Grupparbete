using System.Collections.Generic;
using System.Diagnostics;
using BehaviorTree;

using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class MeeleAttackTreeNode : TreeNode
{
    
    /*
     * Klassen gör så att fienden vänder sig mot sitt mål och sedan attackerar det. Här väljer den också
     * Vilket mål den ska attackera
     * 
     */
    private NavMeshAgent agent;
    private GameObject gameObject;
    private Transform target;
    private List<Transform> targets;
    private Vector3 currentVelocity;
    private float smoothDamp;
    private const int largeDistanceNumber = 100;
    private Animator _animator;
    private MeleeWepon _meleeWepon;

    
    
    public MeeleAttackTreeNode(NavMeshAgent agent, GameObject gameObject, List<Transform> targets, Animator animator, MeleeWepon meleeWepon)
    {
        this.agent = agent;
        this.gameObject = gameObject;
        this.targets = targets;
        smoothDamp = 1f;
        _animator = animator;
        _meleeWepon = meleeWepon;
    }

    public override NodeState Evaluate()
    {
        
        float distance = largeDistanceNumber;
        foreach (Transform target in targets)
        {
            float tempdistance = Vector3.Distance(target.position, agent.transform.position);

            if (tempdistance < distance && target.gameObject.activeInHierarchy )
            {
                distance = tempdistance;
                this.target = target;
            }
        }
        
        
        agent.isStopped = true;
        //ai.SetColor(Color.red);
        Vector3 direction = target.position - gameObject.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(gameObject.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        gameObject.transform.rotation = rotation;

        // TODO: Ändra senare.!!!!!!!
        if (_meleeWepon.timeRemaining <= 0.1f)
        {
            _animator.SetTrigger("Attack");
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}