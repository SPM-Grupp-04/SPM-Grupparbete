using System;
using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


public class MeleeWepon : MonoBehaviour
{
    private float cooldownTime = 0.5f;
    public float timeRemaining;
    private GameObject otherP;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    private void Start()
    {
        timeRemaining = cooldownTime;
    }

    public void HitPlayer()
    {
        var damageEvent = new DealDamageEventInfo(otherP, 1);
        EventSystem.current.FireEvent(damageEvent);
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
       // Debug.Log("Count");
        navMeshAgent.isStopped = true;

        //animator.SetBool("Run", false);
        animator.SetTrigger("Attack");
        otherP = other.gameObject;
    }

  
}