using EgilEventSystem;
using EgilScripts.DieEvents;
using UnityEngine;
using UnityEngine.AI;


public class MeleeWepon : MonoBehaviour
{
    private float cooldownTime = 0.5f;
    public float timeRemaining;
    [SerializeField]private NavMeshAgent navMeshAgent;
    
    private void Start()
    {
        
        timeRemaining = cooldownTime;
    }

    
 
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        navMeshAgent.isStopped = true;
        if (timeRemaining < 0.0f)
        {
            var damageEvent = new DealDamageEventInfo(other.gameObject, 1);
            EventSystem.current.FireEvent(damageEvent);
        }

        if (timeRemaining < 0.0f)
        {
            timeRemaining = cooldownTime;
        }

        timeRemaining -= Time.deltaTime;
    }
}